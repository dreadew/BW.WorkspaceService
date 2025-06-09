using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public class UpdateWorkspaceRequest : BaseDto
{
    [Display(Name = "Название")] 
    public string Name { get; set; }

    [Display(Name = "Признак актуальность")]
    public bool? IsDeleted { get; set; }
}