using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectoryArtifact : IEntity<Guid>, IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public Guid DirectoryId { get; set; } = Guid.Empty;
    public WorkspaceDirectory Directory { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public string? ChangedBy { get; set; }
}