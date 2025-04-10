using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public class CreateWorkspaceRequest
{
    [Display(Name = "Идентификатор пользователя")]
    public string UserId { get; set; }
    [Display(Name = "Название")]
    public string Name { get; set; }
}