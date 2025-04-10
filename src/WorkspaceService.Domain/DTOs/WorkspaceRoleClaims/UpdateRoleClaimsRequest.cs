using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

public class UpdateRoleClaimsRequest
{
    [Display(Name = "Идентификатор")]
    public string Id { get; set; }

    [Display(Name = "Название")]
    public string? Value { get; set; }
}