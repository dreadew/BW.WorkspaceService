using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceRoles;

public class UpdateRoleRequest
{
    [Display(Name = "Идентификатор роли")]
    public string Id { get; set; }
    [Display(Name = "Название")] 
    public string Name { get; set; }
    [Display(Name = "Признак актуальность")]
    public bool? IsDeleted { get; set; }
}