namespace WorkspaceService.Domain.DTOs.WorkspacePositions;

public record class CreatePositionRequest(string Name, 
    string WorkspaceId);