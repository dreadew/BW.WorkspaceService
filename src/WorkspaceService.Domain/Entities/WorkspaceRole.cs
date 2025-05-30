using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceRole : IEntity<string>, IAuditable
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string WorkspaceId { get; set; } = string.Empty;
    public Workspace Workspace { get; set; }
    
    public List<WorkspaceRoleClaim> RoleClaims { get; set; } = new List<WorkspaceRoleClaim>();
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public string? ChangedBy { get; set; }
}