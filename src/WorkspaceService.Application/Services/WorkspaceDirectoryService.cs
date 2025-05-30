using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.File;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Exceptions;
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
        var entity = _mapper.Map<WorkspaceDirectory>(dto);
        entity.Id = Guid.NewGuid().ToString();
        await workspaceDirectoryRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateDirectoryRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directory = await workspaceDirectoryRepository
            .FindMany(x => x.Id == dto.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException("Не удалось найти директорию");
        }
        _mapper.Map(dto, directory);
        await workspaceDirectoryRepository.UpdateAsync(directory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id,
        CancellationToken cancellationToken = default) => await UpdateActualityInternal
        (id, false, cancellationToken);

    public async Task RestoreAsync(string id,
        CancellationToken cancellationToken = default) => await UpdateActualityInternal
        (id, true, cancellationToken);
    
    public async Task<DirectoryDto> GetByIdAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directory = await workspaceDirectoryRepository
            .FindMany(x => x.Id == id)
            .Include(x => x.ChildNesting)
                .ThenInclude(x => x.ChildDirectoryNavigation)
            .Include(x => x.Artifacts)
            .FirstOrDefaultAsync(cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException("Не удалось найти директорию");
        }
        return _mapper.Map<DirectoryDto>(directory);
    }
    
    public async Task<IEnumerable<DirectoryDto>> ListAsync(ListRequest dto,
        string workspaceId, CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directories = await workspaceDirectoryRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .Include(x => x.ChildNesting)
                .ThenInclude(x => x.ChildDirectoryNavigation)
            .Include(x => x.Artifacts)
            .ToListAsync(cancellationToken);
        if (directories == null)
        {
            throw new NotFoundException("Не удалось найти директории");
        }
        return _mapper.Map<IEnumerable<DirectoryDto>>(directories
            .Take(dto.Limit)
            .Skip(dto.Offset));
    }

    public async Task UploadArtifactAsync(string directoryId, FileUploadRequest dto, CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryArtifactRepository = _unitOfWork.Repository<WorkspaceDirectoryArtifact>();
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directory = await workspaceDirectoryRepository
            .FindMany(x => x.Id == directoryId)
            .FirstOrDefaultAsync(cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException("Не найдена папка");
        }

        if (!directory.Workspace.Users.Any(x => x.UserId == dto.FromId))
        {
            throw new ServiceException("У вас нет прав", true);
        }

        var paths = new List<string>() { "workspace", $"{directory.WorkspaceId}", $"{directory.WorkspaceId}" };
        var uploadDto = _mapper.Map<FileUploadDto>(dto);
        uploadDto.Paths = paths;

        var res = await _fileService.UploadFileAsync(uploadDto, cancellationToken);
        var artifact = new WorkspaceDirectoryArtifact()
        {
            Id = Guid.NewGuid().ToString(),
            Name = dto.FileName,
            DirectoryId = directory.Id,
            Path = res.FilePath
        }; 
        await workspaceDirectoryArtifactRepository.CreateAsync(artifact, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteArtifactAsync(string directoryId,
        FileDeleteRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryArtifactRepository = _unitOfWork.Repository<WorkspaceDirectoryArtifact>();
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var artifact = await workspaceDirectoryArtifactRepository
            .FindMany(x => x.Id == dto.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (artifact == null)
        {
            throw new NotFoundException("Артефакт не найден");
        }
        
        var directory = await workspaceDirectoryRepository
            .FindMany(x => x.Id == directoryId)
            .FirstOrDefaultAsync(cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException("Не найдена папка");
        }

        if (!directory.Workspace.Users.Any(x => x.UserId == dto.FromId))
        {
            throw new ServiceException("У вас нет прав", true);
        }

        var deleteDto = new FileDeleteDto()
        {
            FileName = artifact.Path
        };
        
        await _fileService.DeleteFileAsync(deleteDto, cancellationToken);
        await workspaceDirectoryArtifactRepository.DeleteAsync(x => x.Id == dto
            .Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateActualityInternal(string id, bool isDeleted,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directory = await workspaceDirectoryRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException("Не удалось найти директорию");
        }
        
        directory.IsDeleted = isDeleted;
        //await workspaceDirectoryRepository.DeleteAsync(x => x.Id == id, cancellationToken);
        await workspaceDirectoryRepository.UpdateAsync(directory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken); 
    }
}