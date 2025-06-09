using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public class UpdateRoleRequest : BaseDto
{
    [Display(Name = "Название")] 
    public string Name { get; set; }
    [Display(Name = "Признак актуальность")]
    public bool? IsDeleted { get; set; }
}