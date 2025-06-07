using Common.Base.Entities;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectoryArtifact : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public Guid DirectoryId { get; set; } = Guid.Empty;
    public virtual WorkspaceDirectory Directory { get; set; } = new();
}