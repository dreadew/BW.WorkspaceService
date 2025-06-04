namespace WorkspaceService.Domain.Entities;

public class WorkspaceUser
{
    public Guid WorkspaceId { get; set; } = Guid.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid RoleId { get; set; } = Guid.Empty;
    public WorkspaceRole Role { get; set; } = new();
    public Guid PositionId { get; set; } = Guid.Empty;
    public WorkspacePosition Position { get; set; } = new();
}