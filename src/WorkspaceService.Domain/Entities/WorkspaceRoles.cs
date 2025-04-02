using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceRoles : IEntity<string>, IAuditable
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string WorkspaceId { get; set; } = string.Empty;
    public virtual Workspaces Workspace { get; set; }
    
    public virtual List<WorkspaceRoleClaims> RoleClaims { get; set; } = new List<WorkspaceRoleClaims>();
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public string? ChangedBy { get; set; }
}