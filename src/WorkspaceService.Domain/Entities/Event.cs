using Common.Base.Entities;
using WorkspaceService.Domain.Enums;

namespace WorkspaceService.Domain.Entities;

public class Event : BaseEntity
{
    public KafkaTopic EventType { get; set; }
    public string Payload { get; set; } = string.Empty;
    public bool IsSent { get; set; }
}