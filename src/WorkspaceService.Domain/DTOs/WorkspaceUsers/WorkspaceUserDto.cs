namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public record class WorkspaceUserDto(string WorkspaceId,
    string UserId, string RoleId, string PositionId);