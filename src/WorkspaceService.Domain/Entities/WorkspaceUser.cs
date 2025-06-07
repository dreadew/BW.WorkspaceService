namespace WorkspaceService.Domain.Entities;

public class WorkspaceUser
{
    public Guid WorkspaceId { get; set; } = Guid.Empty;
    public virtual Workspace Workspace { get; set; } = new();
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid RoleId { get; set; } = Guid.Empty;
    public virtual WorkspaceRole Role { get; set; } = new();
    public Guid PositionId { get; set; } = Guid.Empty;
    public virtual WorkspacePosition Position { get; set; } = new();
}