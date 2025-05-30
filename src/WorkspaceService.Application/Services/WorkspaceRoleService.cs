using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Exceptions;
using WorkspaceService.Domain.Interfaces;
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
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var entity = _mapper.Map<WorkspaceRole>(dto);
        entity.Id = Guid.NewGuid().ToString();
        await workspaceRolesRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateRoleRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var role = await workspaceRolesRepository
            .FindMany(x => x.Id == dto.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
        {
            throw new NotFoundException("Роль не найдена");
        }
        
        _mapper.Map(dto, role);
        await workspaceRolesRepository.UpdateAsync(role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken =
        default) => await UpdateActualityInternal(id, false, cancellationToken);
    
    public async Task RestoreAsync(string id, CancellationToken cancellationToken =
        default) => await UpdateActualityInternal(id, true, cancellationToken);

    public async Task<RoleDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var role = await workspaceRolesRepository
            .FindMany(x => x.Id == id)
            .Include(x => x.RoleClaims)
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
        {
            throw new NotFoundException("Роль не найдена");
        }
        
        return _mapper.Map<RoleDto>(role);
    }
    
    public async Task<IEnumerable<RoleDto>> ListAsync(ListRequest dto,
        string workspaceId, CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var roles = await workspaceRolesRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .Include(x => x.RoleClaims)
            .ToListAsync(cancellationToken);
        if (roles == null)
        {
            throw new NotFoundException("Роли не найдены");
        }
        
        return _mapper.Map<IEnumerable<RoleDto>>(roles
            .Take(dto.Limit)
            .Skip(dto.Offset));
    }

    private async Task UpdateActualityInternal(string id, bool isDeleted,
        CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var role = await workspaceRolesRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
        {
            throw new NotFoundException("Роль не найдена");
        }
        
        role.IsDeleted = isDeleted;
        //await workspaceRolesRepository.DeleteAsync(x => x.Id == id, cancellationToken);
        await workspaceRolesRepository.UpdateAsync(role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken); 
    }
}