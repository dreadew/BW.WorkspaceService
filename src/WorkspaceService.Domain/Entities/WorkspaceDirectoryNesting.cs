namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectoryNesting
{
    public string ParentDirectoryId { get; set; } = string.Empty;
    public string ChildDirectoryId { get; set; } = string.Empty;
    public virtual WorkspaceDirectory ParentDirectoryNavigation { get; set; }
    public virtual WorkspaceDirectory ChildDirectoryNavigation { get; set; }
}