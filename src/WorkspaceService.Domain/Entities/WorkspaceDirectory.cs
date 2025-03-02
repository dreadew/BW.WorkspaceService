using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectory : IEntity<string>, IAuditable
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<WorkspaceDirectoryNesting> ChildNestings { get; set; } = new List<WorkspaceDirectoryNesting>();
    public List<WorkspaceDirectoryNesting> ParentNestings { get; set; } = new List<WorkspaceDirectoryNesting>();
    public List<WorkspaceDirectoryArtifact> Artifacts { get; set; } = new List<WorkspaceDirectoryArtifact>();
    public string WorkspaceId { get; set; } = string.Empty;
    public Workspaces Workspace { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
}