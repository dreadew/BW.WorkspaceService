using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.Messaging;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Options;
using WorkspaceService.Domain.Repositories;

namespace WorkspaceService.Infrastructure.Messaging;

public class KafkaConsumerService : BackgroundService
{
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IServiceScopeFactory  _scopeFactory;
    private readonly ConsumerConfig _config;

    public KafkaConsumerService(ILogger<KafkaConsumerService> logger,
        IServiceScopeFactory scopeFactory,
        IOptions<KafkaOptions> options)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _config = new ConsumerConfig()
        {
            BootstrapServers = options.Value.BootstrapServers,
            GroupId = options.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();
        consumer.Subscribe([ KafkaConstants.UserChangedActualityTopic ]);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(cancellationToken);
                var message = consumeResult.Message.Value;
                var topic = consumeResult.Topic;
                
                _logger.LogInformation($"Получено сообщение из топика {topic}: {message}");

                if (message == null)
                {
                    continue;
                }

                switch (topic)
                {
                    case KafkaConstants.UserChangedActualityTopic:
                        await HandleUserChangedActualityAsync(message, cancellationToken);
                        consumer.Commit(consumeResult);
                        break;
                }
            }
            catch (OperationCanceledException ex)
            {
                break;
            }
        }
        
        consumer.Close();
    }

    private async Task HandleUserChangedActualityAsync(string message, CancellationToken token = default)
    {
        var deserializedMessage = JsonSerializer.Deserialize<UserChangedActualityDto>(message);
        if (deserializedMessage == null)
        {
            return;
        }
        
        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var workspaceRepo = unitOfWork.Repository<Workspaces>();
        var workspacesToUpdate = await workspaceRepo
            .FindManyAsync(x => x.CreatedBy == deserializedMessage.UserId, token);
        await unitOfWork.BeginTransactionAsync(token);
        try
        {
            foreach (var workspace in workspacesToUpdate)
            {
                workspace.IsDeleted = deserializedMessage.Actuality;
                await HandleWorkspaceRoles(unitOfWork, workspace.Id, workspace.IsDeleted, token);
                await HandleWorkspacePositions(unitOfWork, workspace.Id, workspace.IsDeleted, 
                    token);
                await HandleWorkspaceDirectories(unitOfWork, workspace.Id, workspace.IsDeleted, 
                    token);
                await workspaceRepo.UpdateAsync(workspace, token);
            }

            await unitOfWork.CommitTransactionAsync(token);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(token);
        }
    }
    
    private async Task HandleWorkspaceRoles(IUnitOfWork unitOfWork, string workspaceId, bool actuality, CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = unitOfWork.Repository<WorkspaceRoles>();
        var roles = await workspaceRolesRepository
            .FindManyAsync(x => x.WorkspaceId == workspaceId, cancellationToken);
        foreach (var role in roles)
        {
            role.IsDeleted = actuality;
            await workspaceRolesRepository.UpdateAsync(role, cancellationToken);
        }
    }

    private async Task HandleWorkspacePositions(IUnitOfWork unitOfWork, string workspaceId, bool 
            actuality,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = unitOfWork.Repository<WorkspacePositions>();
        var positions = await workspacePositionsRepository
            .FindManyAsync(x => x.WorkspaceId == workspaceId, cancellationToken);
        foreach (var position in positions)
        {
            position.IsDeleted = actuality;
            await workspacePositionsRepository.UpdateAsync(position, cancellationToken);
        }
    }

    private async Task HandleWorkspaceDirectories(IUnitOfWork unitOfWork, string workspaceId, bool actuality,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoriesRepository = unitOfWork.Repository<WorkspaceDirectory>();
        var directories = await  workspaceDirectoriesRepository
            .FindManyAsync(x => x.WorkspaceId == workspaceId, cancellationToken);
        foreach (var directory in directories)
        {
            directory.IsDeleted = actuality;
            await workspaceDirectoriesRepository.UpdateAsync(directory, cancellationToken);
        }
    }
}