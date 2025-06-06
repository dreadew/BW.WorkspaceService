using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceRole : IEntity<Guid>, IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public Guid WorkspaceId { get; set; } = Guid.Empty;
    public virtual Workspace Workspace { get; set; } = new();
    public virtual List<WorkspaceRoleClaim> RoleClaims { get; set; } = new();
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public string? ChangedBy { get; set; }
}