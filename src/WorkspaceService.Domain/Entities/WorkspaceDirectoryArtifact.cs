using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectoryArtifact : IEntity<string>, IAuditable
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string DirectoryId { get; set; } = string.Empty;
    public virtual WorkspaceDirectory Directory { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
}