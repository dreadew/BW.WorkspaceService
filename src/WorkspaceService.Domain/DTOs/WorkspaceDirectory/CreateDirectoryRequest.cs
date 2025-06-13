using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public class CreateDirectoryRequest : BaseRequestDtoWithName
{
    [Display(Name = "Родительская директория")]
    public string? ParentId { get; set; }
    
    [Display(Name = "Идентификатор создателя")]
    public string FromId { get; set; } = string.Empty;
}