using Common.Base.Entities;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceRoleClaim : BaseEntity
{
    public string Value { get; set; } = string.Empty;
    public Guid RoleId { get; set; } = Guid.Empty;
}