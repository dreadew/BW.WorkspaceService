using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Exceptions;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Application.Services;

public class WorkspacePositionService : IWorkspacePositionsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkspacePositionService> _logger;
    private readonly IMapper _mapper;

    public WorkspacePositionService(IUnitOfWork unitOfWork,
        ILogger<WorkspacePositionService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateAsync(CreatePositionRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var position = _mapper.Map<WorkspacePosition>(dto);
        position.Id = Guid.NewGuid().ToString();
        await workspacePositionsRepository.CreateAsync(position, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdatePositionRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var position = await workspacePositionsRepository
            .FindMany(x => x.Id == dto.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (position == null)
        {
            throw new NotFoundException("Должность не найдена");
        }
        
        _mapper.Map(dto, position);
        await workspacePositionsRepository.UpdateAsync(position, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = 
            default) => await UpdateActualityInternal(id, false, cancellationToken);
    
    public async Task RestoreAsync(string id, CancellationToken cancellationToken = 
        default) => await UpdateActualityInternal(id, true, cancellationToken);

    public async Task<PositionDto> GetByIdAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var position = await workspacePositionsRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (position == null)
        {
            throw new NotFoundException("Должность не найдена");
        }
        
        return _mapper.Map<PositionDto>(position);
    }
    
    public async Task<IEnumerable<PositionDto>> ListAsync(ListRequest dto, string workspaceId,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var positions = await workspacePositionsRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
        if (positions == null)
        {
            throw new NotFoundException("Должности не найдены");
        }
        
        return _mapper.Map<IEnumerable<PositionDto>>(positions
            .Take(dto.Limit)
            .Skip(dto.Offset));
    }

    private async Task UpdateActualityInternal(string id, bool isDeleted,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var position = await workspacePositionsRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (position == null)
        {
            throw new NotFoundException("Должность не найдена");
        }
        
        position.IsDeleted = isDeleted;
        //await workspacePositionsRepository.DeleteAsync(x => x.Id == id, cancellationToken);
        await workspacePositionsRepository.UpdateAsync(position, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken); 
    }
}