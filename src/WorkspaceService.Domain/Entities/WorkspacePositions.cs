using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspacePositions : IEntity<string>, IAuditable
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string WorkspaceId { get; set; } = string.Empty;
    public virtual Workspaces Workspace { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
}