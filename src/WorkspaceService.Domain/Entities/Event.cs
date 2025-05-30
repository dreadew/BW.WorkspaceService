using DefaultNamespace;
using WorkspaceService.Domain.Enums;

namespace WorkspaceService.Domain.Entities;

public class Event : IEntity<string>
{
    public string Id { get; set; }
    public KafkaTopic EventType { get; set; }
    public string Payload { get; set; }
    public bool IsSent { get; set; }
}