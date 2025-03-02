using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public record class UpdateDirectoryRequest(
    [property: Display(Name="Идентификатор папки")] string Id,
    [property: Display(Name="Название")] string? Name);