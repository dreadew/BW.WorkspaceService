using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

public class CreateRoleClaimsRequest
{
    [Display(Name = "Значение")] 
    public string Value { get; set; }

    [Display(Name = "Идентификатор роли")]
    public string RoleId { get; set; }
}