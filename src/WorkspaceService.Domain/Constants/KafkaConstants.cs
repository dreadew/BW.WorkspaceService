namespace WorkspaceService.Domain.Constants;

public static class KafkaConstants
{
    public const string Section = "Kafka";
    public const string BootstrapServers = $"{Section}:BootstrapServers";
    public const string GroupId = $"{Section}:GroupId";

    public const string WorkspaceChangedActualityTopic = "Workspace.ChangedActuality";
    public const string UserChangedActualityTopic = "User.ChangedActuality";
}