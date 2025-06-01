using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public class UpdateWorkspaceRequest
{
    [Display(Name = "Идентификатор")]
    public string Id { get; set; }

    [Display(Name = "Название")] 
    public string Name { get; set; }

    [Display(Name = "Признак актуальность")]
    public bool? IsDeleted { get; set; }
}