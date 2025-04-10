using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public class RestoreWorkspaceRequest
{
    [Display(Name = "Идентификатор пользователя")]
    public string FromId { get; set; }
    [Display(Name = "Идентификатор рабочего пространства")]
    public string WorkspaceId { get; set; }
}