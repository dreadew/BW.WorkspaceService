using System.ComponentModel.DataAnnotations;
using Common.Base.DTO.Entity;

namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public class UpdateDirectoryRequest : BaseDto
{
    [Display(Name="Название")] public string? Name { get; set; }
}