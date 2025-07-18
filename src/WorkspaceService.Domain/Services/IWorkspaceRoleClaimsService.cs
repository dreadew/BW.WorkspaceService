using Common.Base.DTO;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

namespace WorkspaceService.Domain.Services;

public interface IWorkspaceRoleClaimsService
{
    Task CreateAsync(CreateRoleClaimsRequest dto,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateRoleClaimsRequest dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<RoleClaimsDto> GetByIdAsync(Guid id, CancellationToken
        cancellationToken = default);

    Task<(List<RoleClaimsDto>, long)> ListAsync(ListRequest dto,
        Guid roleId, CancellationToken cancellationToken = default);
}