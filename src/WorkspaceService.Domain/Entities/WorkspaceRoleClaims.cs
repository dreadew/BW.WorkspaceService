using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceRoleClaims : IEntity<string>
{
    public string Id { get; set; }
    public string Value { get; set; }
    public string RoleId { get; set; }
    public virtual WorkspaceRoles Role { get; set; }
}