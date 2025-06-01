using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceRoleClaim : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Value { get; set; }
    public Guid RoleId { get; set; }
    public WorkspaceRole Role { get; set; }
}