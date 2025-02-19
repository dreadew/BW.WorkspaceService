using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceRoleClaims : IEntity<string>
{
    public string Id { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
    public string RoleId { get; set; }
    public WorkspaceRoles Role { get; set; }
}