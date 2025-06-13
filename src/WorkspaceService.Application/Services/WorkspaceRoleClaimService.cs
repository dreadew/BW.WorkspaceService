using AutoMapper;
using Common.Base.DTO;
using Common.Base.Exceptions;
using Common.Base.Extensions;
using Common.Base.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;
using WorkspaceService.Domain.Entities;
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
        var workspaceRoleRepo = _unitOfWork.Repository<WorkspaceRole>();
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaim>();
        var role  = await workspaceRoleRepo
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.RoleNotFound);
        }
        var entity = _mapper.Map<WorkspaceRoleClaim>(dto);
        role.Claims.Add(entity);
        await workspaceRoleClaimsRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateRoleClaimsRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaim>();
        var claim = await workspaceRoleClaimsRepository
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (claim == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.ClaimNotFound);
        }
        
        _mapper.Map(dto, claim);
        workspaceRoleClaimsRepository.Update(claim, cancellationToken);
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
        var claim = await workspaceRoleClaimsRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (claim == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.ClaimNotFound);
        }
        
        return _mapper.Map<RoleClaimsDto>(claim);
    }
    
    public async Task<(List<RoleClaimsDto>, long)> ListAsync(ListRequest dto,
        Guid roleId, CancellationToken cancellationToken = default)
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaim>();
        var claims = workspaceRoleClaimsRepository.FindMany(x => x.RoleId == roleId);
        if (claims == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.ClaimsNotFound);
        }

        var count = claims.Count();
        
        return (_mapper.Map<List<RoleClaimsDto>>(await claims.Paging(dto)
            .ToListAsync(cancellationToken)), count);
    }
}