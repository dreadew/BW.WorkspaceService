using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public class CreateDirectoryRequest
{
    [Display(Name = "Название")]
    public string Name { get; set; }

    [Display(Name = "Идентификатор рабочего пространства")]
    public string WorkspaceId { get; set; }
}