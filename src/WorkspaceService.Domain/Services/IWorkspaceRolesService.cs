using Common.Base.DTO;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;

namespace WorkspaceService.Domain.Services;

public interface IWorkspaceRolesService
{
    Task CreateAsync(CreateRoleRequest dto,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateRoleRequest dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task RestoreAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<RoleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<RoleDto>> ListAsync(ListRequest dto,
        Guid workspaceId, CancellationToken cancellationToken = default);
}