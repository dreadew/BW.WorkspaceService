using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public record class RoleDto(string Id,
    string Name,
    List<RoleClaimsDto> Claims,
    DateTime CreatedAt,
    DateTime? ModifiedAt);