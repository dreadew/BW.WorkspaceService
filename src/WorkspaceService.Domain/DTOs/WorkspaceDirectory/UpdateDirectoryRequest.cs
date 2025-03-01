using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public record class UpdateDirectoryRequest(
    [Display(Name="Идентификатор папки")] string Id,
    [Display(Name="Название")] string? Name);