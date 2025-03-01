namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public class DeleteUserRequest
{
    public string WorkspaceId { get; set; }
    public string UserId { get; set; }
}