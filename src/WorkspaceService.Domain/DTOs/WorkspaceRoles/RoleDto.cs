using Common.Base.DTO.Entity;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public class RoleDto : BaseSoftDeletableDtoWithName 
{
    public List<RoleClaimsDto> Claims { get; set; }
}