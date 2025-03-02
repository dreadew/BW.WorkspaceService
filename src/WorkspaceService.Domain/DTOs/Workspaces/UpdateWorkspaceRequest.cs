using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public record class UpdateWorkspaceRequest(
    [property: Display(Name="Идентификатор")] string Id,
    [property: Display(Name="Название")] string Name);