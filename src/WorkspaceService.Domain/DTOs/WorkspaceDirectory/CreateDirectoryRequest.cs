using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public record class CreateDirectoryRequest(
    [Display(Name="Название")] string Name,
    [Display(Name="Идентификатор рабочего пространства")] string WorkspaceId);