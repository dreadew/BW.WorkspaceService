using Microsoft.Extensions.Logging;
using Quartz;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Infrastructure.Jobs;

public class SendMessageJob : IJob
{
    private readonly ILogger<SendMessageJob> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKafkaProducerService _kafkaProducerService;

    public SendMessageJob(ILogger<SendMessageJob> logger, IUnitOfWork unitOfWork,
        IKafkaProducerService kafkaProducerService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _kafkaProducerService = kafkaProducerService;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var eventsRepo = _unitOfWork.Repository<Events>();
        var events = await eventsRepo.FindManyAsync(x => !x.IsSent);
        foreach (var newEvent in events)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _kafkaProducerService.PublishAsync(newEvent.EventType.ToString(), newEvent.Payload);
                newEvent.IsSent = true;
                await eventsRepo.UpdateAsync(newEvent);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
        }
    }
}