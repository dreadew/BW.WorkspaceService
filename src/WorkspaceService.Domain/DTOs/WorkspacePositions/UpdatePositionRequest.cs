using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspacePositions;

public class UpdatePositionRequest
{
    [Display(Name="Идентификатор должности")] 
    public string Id { get; set; }
    [Display(Name="Название")] 
    public string Name { get; set; }
    [Display(Name="Признак актуальность")] 
    public bool? IsDeleted { get; set; }
}