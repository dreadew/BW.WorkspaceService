using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public class UpdateDirectoryRequest
{
    [Display(Name="Идентификатор папки")] public string Id { get; set; }
    [Display(Name="Название")] public string? Name { get; set; }
}