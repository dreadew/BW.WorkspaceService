using Common.Base.DTO.Entity;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public class RoleDto : BaseSoftDeletableDto
{
    public string Name { get; set; }
    public List<RoleClaimsDto> Claims { get; set; }
}