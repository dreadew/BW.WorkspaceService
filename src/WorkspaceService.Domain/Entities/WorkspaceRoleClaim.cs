using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceRoleClaim : IEntity<string>
{
    public string Id { get; set; }
    public string Value { get; set; }
    public string RoleId { get; set; }
    public WorkspaceRole Role { get; set; }
}