using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public record class CreateWorkspaceRequest(
    [Display(Name="Идентификатор пользователя")] string UserId,
    [Display(Name="Название")] string Name);