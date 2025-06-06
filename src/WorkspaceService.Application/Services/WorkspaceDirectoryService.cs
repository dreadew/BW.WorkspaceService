using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.File;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Exceptions;
using WorkspaceService.Domain.Extensions;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Application.Services;

public class WorkspaceDirectoryService : IWorkspaceDirectoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkspaceDirectoryService> _logger;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public WorkspaceDirectoryService(
        IUnitOfWork unitOfWork,
        ILogger<WorkspaceDirectoryService> logger,
        IMapper mapper,
        IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task CreateAsync(CreateDirectoryRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var workspaceDirectoryNestingRepository = _unitOfWork.Repository<WorkspaceDirectoryNesting>();
        var entity = _mapper.Map<WorkspaceDirectory>(dto);
        await workspaceDirectoryRepository.CreateAsync(entity, cancellationToken);
        if (!string.IsNullOrEmpty(dto.ParentId) && Guid.TryParse(dto.ParentId, out var parentId))
        {
            var nesting = new WorkspaceDirectoryNesting
            {
                ParentDirectoryId = parentId,
                ChildDirectoryId = entity.Id
            };
            await workspaceDirectoryNestingRepository.CreateAsync(nesting, cancellationToken);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateDirectoryRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directory = await workspaceDirectoryRepository
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.DirectoryNotFound);
        }
        _mapper.Map(dto, directory);
        workspaceDirectoryRepository.Update(directory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id,
        CancellationToken cancellationToken = default) => await UpdateActualityInternal
        (id, false, cancellationToken);

    public async Task RestoreAsync(Guid id,
        CancellationToken cancellationToken = default) => await UpdateActualityInternal
        (id, true, cancellationToken);
    
    public async Task<DirectoryDto> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directory = await workspaceDirectoryRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.DirectoryNotFound);
        }
        var dto = _mapper.Map<DirectoryDto>(directory);
        dto.Children = directory.ChildNesting?.Select(n => _mapper.Map<DirectoryDto>(n.ChildDirectoryNavigation)).ToList() ?? new();
        dto.Parent = directory.ParentNesting?.FirstOrDefault() != null
            ? _mapper.Map<DirectoryDto>(directory.ParentNesting.First().ParentDirectoryNavigation)
            : null;
        return dto;
    }
    
    public async Task<IEnumerable<DirectoryDto>> ListAsync(ListRequest dto,
        Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directories = await workspaceDirectoryRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .WhereIf(!dto.IncludeDeleted, d => !d.IsDeleted)
            .Paging(dto)
            .ToListAsync(cancellationToken);
        if (directories == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.DirectoriesNotFound);
        }
        var result = directories
            .Take(dto.Limit)
            .Skip(dto.Offset)
            .Select(directory => {
                var dirDto = _mapper.Map<DirectoryDto>(directory);
                dirDto.Children = directory.ChildNesting?.Select(n => _mapper.Map<DirectoryDto>(n.ChildDirectoryNavigation)).ToList() ?? new();
                dirDto.Parent = directory.ParentNesting?.FirstOrDefault() != null
                    ? _mapper.Map<DirectoryDto>(directory.ParentNesting.First().ParentDirectoryNavigation)
                    : null;
                return dirDto;
            });
        return result;
    }

    public async Task UploadArtifactAsync(Guid directoryId, FileUploadRequest dto, CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryArtifactRepository = _unitOfWork.Repository<WorkspaceDirectoryArtifact>();
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directory = await workspaceDirectoryRepository
            .FindMany(x => x.Id == directoryId)
            .FirstOrDefaultAsync(cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.FolderNotFound);
        }

        if (!directory.Workspace.Users.Any(x => x.UserId == Guid.Parse(dto.FromId)))
        {
            throw new ServiceException(ExceptionResourceKeys.NoRights, true);
        }

        var paths = new List<string>() { "workspace", $"{directory.WorkspaceId}", $"{directory.WorkspaceId}" };
        var uploadDto = _mapper.Map<FileUploadDto>(dto);
        uploadDto.Paths = paths;

        var res = await _fileService.UploadFileAsync(uploadDto, cancellationToken);
        var artifact = new WorkspaceDirectoryArtifact()
        {
            Name = dto.FileName,
            DirectoryId = directory.Id,
            Path = res.FilePath
        }; 
        await workspaceDirectoryArtifactRepository.CreateAsync(artifact, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteArtifactAsync(Guid directoryId,
        FileDeleteRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryArtifactRepository = _unitOfWork.Repository<WorkspaceDirectoryArtifact>();
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var artifact = await workspaceDirectoryArtifactRepository
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (artifact == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.ArtifactNotFound);
        }
        
        var directory = await workspaceDirectoryRepository
            .FindMany(x => x.Id == directoryId)
            .FirstOrDefaultAsync(cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.FolderNotFound);
        }

        if (!directory.Workspace.Users.Any(x => x.UserId == Guid.Parse(dto.FromId)))
        {
            throw new ServiceException(ExceptionResourceKeys.NoRights, true);
        }

        var deleteDto = new FileDeleteDto()
        {
            FileName = artifact.Path
        };
        
        await _fileService.DeleteFileAsync(deleteDto, cancellationToken);
        await workspaceDirectoryArtifactRepository
            .DeleteAsync(x => x.Id == Guid.Parse(dto.Id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateActualityInternal(Guid id, bool isDeleted,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directory = await workspaceDirectoryRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.DirectoryNotFound);
        }
        
        directory.IsDeleted = isDeleted;
        //await workspaceDirectoryRepository.DeleteAsync(x => x.Id == id, cancellationToken);
        workspaceDirectoryRepository.Update(directory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken); 
    }
}