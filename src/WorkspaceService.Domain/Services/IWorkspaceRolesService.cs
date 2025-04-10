using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;

namespace WorkspaceService.Domain.Services;

public interface IWorkspaceRolesService
{
    Task CreateAsync(CreateRoleRequest dto,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateRoleRequest dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);

    Task RestoreAsync(string id, CancellationToken cancellationToken = default);
    
    Task<RoleDto> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<IEnumerable<RoleDto>> ListAsync(ListRequest dto,
        string workspaceId, CancellationToken cancellationToken = default);
}