using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectory : IEntity<string>, IAuditable
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<WorkspaceDirectoryNesting> ChildNesting { get; set; } = new List<WorkspaceDirectoryNesting>();
    public List<WorkspaceDirectoryNesting> ParentNesting { get; set; } = new List<WorkspaceDirectoryNesting>();
    public List<WorkspaceDirectoryArtifact> Artifacts { get; set; } = new List<WorkspaceDirectoryArtifact>();
    public bool IsDeleted { get; set; }
    public string WorkspaceId { get; set; } = string.Empty;
    public Workspace Workspace { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public string? ChangedBy { get; set; }
}