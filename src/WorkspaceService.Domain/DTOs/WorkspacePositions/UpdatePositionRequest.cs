namespace WorkspaceService.Domain.DTOs.WorkspacePositions;

public record class UpdatePositionRequest(string Id,
    string Name, string WorkspaceId);