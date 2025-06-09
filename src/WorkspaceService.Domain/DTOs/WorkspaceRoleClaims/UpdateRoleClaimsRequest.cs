using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

public class UpdateRoleClaimsRequest : BaseDto
{
    [Display(Name = "Название")]
    public string? Value { get; set; }
}