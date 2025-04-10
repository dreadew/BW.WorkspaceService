using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Options;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Infrastructure.Messaging;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IOptions<KafkaOptions> _options;
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducerService> _logger;

    public KafkaProducerService(IOptions<KafkaOptions> options,
        ILogger<KafkaProducerService> logger)
    {
        _options = options;
        _logger = logger;
        var config = new ProducerConfig() { BootstrapServers = _options.Value.BootstrapServers };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishWithSerializationAsync<T>(string topic, T message)
    {
        var serializedMessage = JsonSerializer.Serialize(message);
        await PublishAsync(topic, serializedMessage);
    }

    public async Task PublishAsync(string topic, string message) => await _producer.ProduceAsync(topic, 
            new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = message });
}