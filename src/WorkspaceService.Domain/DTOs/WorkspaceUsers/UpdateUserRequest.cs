namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public record class UpdateUserRequest(string WorkspaceId,
    string UserId, string? RoleId, string? PositionId);