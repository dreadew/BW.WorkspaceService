using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public class RestoreWorkspaceRequest
{
    [Display(Name = "Идентификатор рабочего пространства")]
    public string WorkspaceId { get; set; }
}