using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public record class CreateWorkspaceRequest(
    [property: Display(Name="Идентификатор пользователя")] string UserId,
    [property: Display(Name="Название")] string Name);