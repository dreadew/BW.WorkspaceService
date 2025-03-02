namespace WorkspaceService.Domain.Entities;

public class WorkspaceDirectoryNesting
{
    public string ParentDirectoryId { get; set; } = string.Empty;
    public string ChildDirectoryId { get; set; } = string.Empty;
    public WorkspaceDirectory ParentDirectoryNavigation { get; set; }
    public WorkspaceDirectory ChildDirectoryNavigation { get; set; }
}