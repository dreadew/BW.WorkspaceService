using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public class CreateDirectoryRequest : BaseDto
{
    [Display(Name = "Название")]
    public string Name { get; set; }

    [Display(Name = "Родительская директория")]
    public string? ParentId { get; set; }
}