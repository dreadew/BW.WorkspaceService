using Common.Base.Entities;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceRole : BaseSoftDeletableEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid WorkspaceId { get; set; } = Guid.Empty;
    public virtual Workspace Workspace { get; set; } = new();
    public virtual List<WorkspaceRoleClaim> Claims { get; set; } = new();
}