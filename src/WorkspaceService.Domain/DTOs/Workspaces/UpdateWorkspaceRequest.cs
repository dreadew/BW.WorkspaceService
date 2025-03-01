using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.Workspaces;

public record class UpdateWorkspaceRequest(
    [Display(Name="Идентификатор")] string Id,
    [Display(Name="Название")] string Name);