namespace WorkspaceService.Domain.Entities;

public class WorkspaceUser
{
    public Guid WorkspaceId { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public WorkspaceRole Role { get; set; }
    public Guid PositionId { get; set; }
    public WorkspacePosition Position { get; set; }
}