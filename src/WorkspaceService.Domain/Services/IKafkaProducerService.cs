namespace WorkspaceService.Domain.Services;

public interface IKafkaProducerService
{
    Task PublishAsync<T>(string topic, T message);
}