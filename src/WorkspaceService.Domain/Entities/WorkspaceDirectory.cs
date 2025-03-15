using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectory : IEntity<string>, IAuditable
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public virtual List<WorkspaceDirectoryNesting> ChildNesting { get; set; } = new List<WorkspaceDirectoryNesting>();
    public virtual List<WorkspaceDirectoryNesting> ParentNesting { get; set; } = new List<WorkspaceDirectoryNesting>();
    public virtual List<WorkspaceDirectoryArtifact> Artifacts { get; set; } = new List<WorkspaceDirectoryArtifact>();
    public string WorkspaceId { get; set; } = string.Empty;
    public virtual Workspaces Workspace { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
}