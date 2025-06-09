using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public class CreateRoleRequest : BaseDto
{
    [Display(Name = "Название")] 
    public string Name { get; set; }
}