namespace WorkspaceService.Domain.Services;

public interface IKafkaProducerService
{
    Task PublishWithSerializationAsync<T>(string topic, T message);
    Task PublishAsync(string topic, string message);
}