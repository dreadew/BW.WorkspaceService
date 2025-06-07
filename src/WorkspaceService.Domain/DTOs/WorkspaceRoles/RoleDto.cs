using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public class RoleDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<RoleClaimsDto> Claims { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}