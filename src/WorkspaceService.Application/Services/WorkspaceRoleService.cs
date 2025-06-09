using AutoMapper;
using Common.Base.DTO;
using Common.Base.Exceptions;
using Common.Base.Extensions;
using Common.Base.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Application.Services;

public class WorkspaceRoleService : IWorkspaceRolesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkspaceRoleService> _logger;
    private readonly IMapper _mapper;

    public WorkspaceRoleService(IUnitOfWork unitOfWork,
        ILogger<WorkspaceRoleService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateAsync(CreateRoleRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepo = _unitOfWork.Repository<Workspace>();
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var workspace  = await workspaceRepo
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }
        var entity = _mapper.Map<WorkspaceRole>(dto);
        await workspaceRolesRepository.CreateAsync(entity, cancellationToken);
        workspace.Roles.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateRoleRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var role = await workspaceRolesRepository
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.RoleNotFound);
        }
        
        _mapper.Map(dto, role);
        workspaceRolesRepository.Update(role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken =
        default) => await UpdateActualityInternal(id, true, cancellationToken);
    
    public async Task RestoreAsync(Guid id, CancellationToken cancellationToken =
        default) => await UpdateActualityInternal(id, false, cancellationToken);

    public async Task<RoleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var role = await workspaceRolesRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.RoleNotFound);
        }
        
        return _mapper.Map<RoleDto>(role);
    }
    
    public async Task<IEnumerable<RoleDto>> ListAsync(ListRequest dto,
        Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var roles = await workspaceRolesRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .WhereIf(!dto.IncludeDeleted, d => !d.IsDeleted)
            .Paging(dto)
            .ToListAsync(cancellationToken);
        if (roles == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.RolesNotFound);
        }
        
        return _mapper.Map<IEnumerable<RoleDto>>(roles
            .Take(dto.Limit)
            .Skip(dto.Offset));
    }

    private async Task UpdateActualityInternal(Guid id, bool isDeleted,
        CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var role = await workspaceRolesRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.RoleNotFound);
        }
        
        role.IsDeleted = isDeleted;
        //await workspaceRolesRepository.DeleteAsync(x => x.Id == id, cancellationToken);
        workspaceRolesRepository.Update(role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken); 
    }
}