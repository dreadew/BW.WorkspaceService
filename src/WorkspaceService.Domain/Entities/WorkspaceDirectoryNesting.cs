using DefaultNamespace;

namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectoryNesting : IEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ParentDirectoryId { get; set; } = Guid.Empty;
    public Guid ChildDirectoryId { get; set; } = Guid.Empty;
    public virtual WorkspaceDirectory ParentDirectoryNavigation { get; set; } = new();
    public virtual WorkspaceDirectory ChildDirectoryNavigation { get; set; } = new();
}