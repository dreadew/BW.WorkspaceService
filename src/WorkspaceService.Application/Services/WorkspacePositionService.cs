using AutoMapper;
using Common.Base.DTO;
using Common.Base.Exceptions;
using Common.Base.Extensions;
using Common.Base.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.Entities;
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
        var workspaceRepo = _unitOfWork.Repository<Workspace>();
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var workspace  = await workspaceRepo
            .FindMany(x => x.Id == Guid.Parse(dto.WorkspaceId))
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }
        var position = _mapper.Map<WorkspacePosition>(dto);
        position.Workspace = workspace;
        await workspacePositionsRepository.CreateAsync(position, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdatePositionRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var position = await workspacePositionsRepository
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (position == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.PositionNotFound);
        }
        
        _mapper.Map(dto, position);
        workspacePositionsRepository.Update(position, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = 
            default) => await UpdateActualityInternal(id, true, cancellationToken);
    
    public async Task RestoreAsync(Guid id, CancellationToken cancellationToken = 
        default) => await UpdateActualityInternal(id, false, cancellationToken);

    public async Task<PositionDto> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var position = await workspacePositionsRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (position == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.PositionNotFound);
        }
        
        return _mapper.Map<PositionDto>(position);
    }
    
    public async Task<IEnumerable<PositionDto>> ListAsync(ListRequest dto, Guid workspaceId,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var positions = await workspacePositionsRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .WhereIf(!dto.IncludeDeleted, d => !d.IsDeleted)
            .Paging(dto)
            .ToListAsync(cancellationToken);
        if (positions == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.PositionsNotFound);
        }
        
        return _mapper.Map<IEnumerable<PositionDto>>(positions
            .Take(dto.Limit)
            .Skip(dto.Offset));
    }

    private async Task UpdateActualityInternal(Guid id, bool isDeleted,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var position = await workspacePositionsRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (position == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.PositionNotFound);
        }
        
        position.IsDeleted = isDeleted;
        //await workspacePositionsRepository.DeleteAsync(x => x.Id == id, cancellationToken);
        workspacePositionsRepository.Update(position, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken); 
    }
}