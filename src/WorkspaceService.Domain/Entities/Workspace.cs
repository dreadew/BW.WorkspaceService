using Common.Base.Entities;

namespace WorkspaceService.Domain.Entities;

public class Workspace : BaseSoftDeletableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? PicturePath { get; set; }
    public virtual List<WorkspaceRole> Roles { get; set; } = new();
    public virtual List<WorkspacePosition> Positions { get; set; } = new();
    public virtual List<WorkspaceUser> Users { get; set; } = new();
    public virtual List<WorkspaceDirectory> Directories { get; set; } = new();
    public Guid CreatedBy { get; set; } = Guid.Empty;
}