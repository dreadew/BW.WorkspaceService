using Common.Base.Entities;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectory : BaseSoftDeletableEntity
{
    public string Name { get; set; } = string.Empty;
    public virtual List<WorkspaceDirectoryNesting> ChildNesting { get; set; } = new();
    public virtual List<WorkspaceDirectoryNesting> ParentNesting { get; set; } = new();
    public virtual List<WorkspaceDirectoryArtifact> Artifacts { get; set; } = new();
    public Guid WorkspaceId { get; set; } = Guid.Empty;
    public virtual Workspace Workspace { get; set; } = new();
}