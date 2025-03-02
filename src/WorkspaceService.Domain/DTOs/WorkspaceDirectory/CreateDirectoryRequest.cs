using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public record class CreateDirectoryRequest(
    [property: Display(Name="Название")] string Name,
    [property: Display(Name="Идентификатор рабочего пространства")] string WorkspaceId);