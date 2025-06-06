using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceRoleClaim : IEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Value { get; set; } = string.Empty;
    public Guid RoleId { get; set; } = Guid.Empty;
    public virtual WorkspaceRole Role { get; set; } = new();
}