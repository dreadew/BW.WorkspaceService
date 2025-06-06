using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Exceptions;
using WorkspaceService.Domain.Extensions;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Application.Services;

public class WorkspaceRoleClaimService : IWorkspaceRoleClaimsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkspaceRoleClaimService> _logger;
    private readonly IMapper _mapper;

    public WorkspaceRoleClaimService(IUnitOfWork unitOfWork,
        ILogger<WorkspaceRoleClaimService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateAsync(CreateRoleClaimsRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaim>();
        var entity = _mapper.Map<WorkspaceRoleClaim>(dto);
        entity.Id = Guid.NewGuid();
        await workspaceRoleClaimsRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateRoleClaimsRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaim>();
        var role = await workspaceRoleClaimsRepository
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.ClaimNotFound);
        }
        
        _mapper.Map(dto, role);
        workspaceRoleClaimsRepository.Update(role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) 
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaim>();
        var claim = await workspaceRoleClaimsRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (claim == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.ClaimNotFound);
        }

        await workspaceRoleClaimsRepository.DeleteAsync(x => x.Id == id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<RoleClaimsDto> GetByIdAsync(Guid id, CancellationToken 
            cancellationToken = default)
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaim>();
        var role = await workspaceRoleClaimsRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.ClaimNotFound);
        }
        
        return _mapper.Map<RoleClaimsDto>(role);
    }
    
    public async Task<IEnumerable<RoleClaimsDto>> ListAsync(ListRequest dto,
        Guid roleId, CancellationToken cancellationToken = default)
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaim>();
        var roles = await workspaceRoleClaimsRepository
            .FindMany(x => x.RoleId == roleId)
            .Paging(dto)
            .ToListAsync(cancellationToken);
        if (roles == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.ClaimsNotFound);
        }
        
        return _mapper.Map<IEnumerable<RoleClaimsDto>>(roles
            .Take(dto.Limit)
            .Skip(dto.Offset));
    }
}