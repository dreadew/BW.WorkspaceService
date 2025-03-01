using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspacePositions;

public record class CreatePositionRequest(
    [Display(Name="Название")] string Name, 
    [Display(Name="Идентификатор рабочего пространства")] string WorkspaceId);