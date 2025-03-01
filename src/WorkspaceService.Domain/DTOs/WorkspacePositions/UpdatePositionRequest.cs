using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspacePositions;

public record class UpdatePositionRequest(
    [Display(Name="Идентификатор должности")] string Id,
    [Display(Name="Название")] string Name, 
    [Display(Name="Идентификатор рабочего пространства")] string WorkspaceId);