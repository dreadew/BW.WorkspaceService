using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public record class DeleteWorkspaceRequest(
    [property: Display(Name="Идентификатор пользователя")] string FromId,
    [property: Display(Name="Идентификатор рабочего пространства")] string WorkspaceId);