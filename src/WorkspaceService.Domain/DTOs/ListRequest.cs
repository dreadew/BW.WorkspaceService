namespace WorkspaceService.Domain.DTOs;

public class ListRequest
{
    public int Limit { get; set; } = 20;
    public int Offset { get; set; } = 0;
    public bool IncludeDeleted { get; set; } = false;
}