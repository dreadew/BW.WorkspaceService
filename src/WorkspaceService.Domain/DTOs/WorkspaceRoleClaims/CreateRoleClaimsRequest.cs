using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

public class CreateRoleClaimsRequest : BaseDto
{
    [Display(Name = "Значение")] 
    public string Value { get; set; }
}