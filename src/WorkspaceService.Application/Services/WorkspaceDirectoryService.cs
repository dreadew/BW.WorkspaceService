using AutoMapper;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.DTOs;
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

    public WorkspaceDirectoryService(
        IUnitOfWork unitOfWork,
        ILogger<WorkspaceDirectoryService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
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
        var directory = await workspaceDirectoryRepository.FindAsync(x => x.Id == dto.Id,
            cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException("Не удалось найти директорию");
        }
        _mapper.Map(dto, directory);
        await workspaceDirectoryRepository.UpdateAsync(directory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directory = await workspaceDirectoryRepository.FindAsync(x => x.Id == id,
            cancellationToken);
        if (directory == null)
        {
            throw new NotFoundException("Не удалось найти директорию");
        }
        
        directory.IsDeleted = true;
        //await workspaceDirectoryRepository.DeleteAsync(x => x.Id == id, cancellationToken);
        await workspaceDirectoryRepository.UpdateAsync(directory, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<DirectoryDto> GetByIdAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directory = await workspaceDirectoryRepository.FindAsync(x => x.Id == id,
            cancellationToken);
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
        var directories = await workspaceDirectoryRepository.FindManyAsync(
            x => x.WorkspaceId == workspaceId, cancellationToken);
        if (directories == null)
        {
            throw new NotFoundException("Не удалось найти директории");
        }
        return _mapper.Map<IEnumerable<DirectoryDto>>(directories
            .Take(dto.Limit)
            .Skip(dto.Offset));
    }
}