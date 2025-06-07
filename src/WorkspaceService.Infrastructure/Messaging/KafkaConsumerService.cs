using System.Text.Json;
using Common.Base.Constants;
using Common.Base.Options;
using Common.Base.Services;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkspaceService.Domain.DTOs.Messaging;
using WorkspaceService.Domain.Entities;

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
               //break;
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

        var workspaceRepo = unitOfWork.Repository<Workspace>();
        var workspacesToUpdate = await workspaceRepo
            .FindMany(x => x.CreatedBy == Guid.Parse(deserializedMessage.UserId))
            .ToListAsync(token);
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
                workspaceRepo.Update(workspace, token);
            }

            await unitOfWork.CommitTransactionAsync(token);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(token);
        }
    }
    
    private async Task HandleWorkspaceRoles(IUnitOfWork unitOfWork, Guid workspaceId, bool actuality, CancellationToken token = default)
    {
        var workspaceRolesRepository = unitOfWork.Repository<WorkspaceRole>();
        var roles = await workspaceRolesRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .ToListAsync(token);
        foreach (var role in roles)
        {
            role.IsDeleted = actuality;
            workspaceRolesRepository.Update(role, token);
        }
    }

    private async Task HandleWorkspacePositions(IUnitOfWork unitOfWork, Guid workspaceId, 
        bool actuality, CancellationToken token = default)
    {
        var workspacePositionsRepository = unitOfWork.Repository<WorkspacePosition>();
        var positions = await workspacePositionsRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .ToListAsync(token);
        foreach (var position in positions)
        {
            position.IsDeleted = actuality;
            workspacePositionsRepository.Update(position, token);
        }
    }

    private async Task HandleWorkspaceDirectories(IUnitOfWork unitOfWork, Guid workspaceId, bool actuality,
        CancellationToken token = default)
    {
        var workspaceDirectoriesRepository = unitOfWork.Repository<WorkspaceDirectory>();
        var directories = await  workspaceDirectoriesRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .ToListAsync(token);
        foreach (var directory in directories)
        {
            directory.IsDeleted = actuality;
            workspaceDirectoriesRepository.Update(directory, token);
        }
    }
}