namespace WorkspaceService.Domain.DTOs.WorkspacePositions;

public record class PositionDto(string Id,
    string Name,
    DateTime CreatedAt,
    DateTime? ModifiedAt);