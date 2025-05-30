namespace WorkspaceService.Domain.Entities;

public class WorkspaceUser
{
    public string WorkspaceId { get; set; }
    public string UserId { get; set; }
    public string RoleId { get; set; }
    public virtual WorkspaceRole Role { get; set; }
    public string PositionId { get; set; }
    public virtual WorkspacePosition Position { get; set; }
}