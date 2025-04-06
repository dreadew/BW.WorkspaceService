using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspacePositions;

public class CreatePositionRequest{
    [Display(Name="Название")] 
    public string Name { get; set; }

    [Display(Name = "Идентификатор рабочего пространства")]
    public string WorkspaceId { get; set; }
}