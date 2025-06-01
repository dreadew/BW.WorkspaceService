namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectoryNesting
{
    public Guid ParentDirectoryId { get; set; } = Guid.Empty;
    public Guid ChildDirectoryId { get; set; } = Guid.Empty;
    public WorkspaceDirectory ParentDirectoryNavigation { get; set; }
    public WorkspaceDirectory ChildDirectoryNavigation { get; set; }
}