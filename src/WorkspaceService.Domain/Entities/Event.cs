using DefaultNamespace;
using WorkspaceService.Domain.Enums;

namespace WorkspaceService.Domain.Entities;

public class Event : IEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public KafkaTopic EventType { get; set; }
    public string Payload { get; set; } = string.Empty;
    public bool IsSent { get; set; }
}