using Common.Base.Entities;

namespace WorkspaceService.Domain.Entities;

public class WorkspacePosition : BaseSoftDeletableEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid WorkspaceId { get; set; } = Guid.Empty;
}