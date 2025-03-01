namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public record class InviteUserRequest(string Id, string UserId,
    string Email);