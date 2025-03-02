using System.ComponentModel.DataAnnotations;

namespace WorkspaceService.Domain.DTOs.WorkspacePositions;

public record class CreatePositionRequest(
    [property: Display(Name="Название")] string Name, 
    [property: Display(Name="Идентификатор рабочего пространства")] string WorkspaceId);