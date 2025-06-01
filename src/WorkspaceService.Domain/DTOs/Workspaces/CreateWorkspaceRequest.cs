using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public class CreateWorkspaceRequest
{
    [Display(Name = "Название")]
    public string Name { get; set; }
}