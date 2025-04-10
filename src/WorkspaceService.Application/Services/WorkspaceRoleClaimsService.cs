using AutoMapper;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Exceptions;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Application.Services;

public class WorkspaceRoleClaimsService : IWorkspaceRoleClaimsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkspaceRoleClaimsService> _logger;
    private readonly IMapper _mapper;

    public WorkspaceRoleClaimsService(IUnitOfWork unitOfWork,
        ILogger<WorkspaceRoleClaimsService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateAsync(CreateRoleClaimsRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaims>();
        var entity = _mapper.Map<WorkspaceRoleClaims>(dto);
        entity.Id = Guid.NewGuid().ToString();
        await workspaceRoleClaimsRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateRoleClaimsRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaims>();
        var role = await workspaceRoleClaimsRepository.GetByIdAsync(dto.Id, 
            cancellationToken);
        if (role == null)
        {
            throw new NotFoundException("Клейм не найден");
        }
        
        _mapper.Map(dto, role);
        await workspaceRoleClaimsRepository.UpdateAsync(role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default) 
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaims>();
        var claim = await workspaceRoleClaimsRepository.GetByIdAsync(id, 
            cancellationToken);
        if (claim == null)
        {
            throw new NotFoundException("Клейм не найден");
        }

        await workspaceRoleClaimsRepository.DeleteAsync(x => x.Id == id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<RoleClaimsDto> GetByIdAsync(string id, CancellationToken 
            cancellationToken = default)
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaims>();
        var role = await workspaceRoleClaimsRepository.GetByIdAsync(id, 
            cancellationToken);
        if (role == null)
        {
            throw new NotFoundException("Клейм не найден");
        }
        
        return _mapper.Map<RoleClaimsDto>(role);
    }
    
    public async Task<IEnumerable<RoleClaimsDto>> ListAsync(ListRequest dto,
        string roleId, CancellationToken cancellationToken = default)
    {
        var workspaceRoleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaims>();
        var roles = await workspaceRoleClaimsRepository.FindManyAsync(x => x.RoleId == roleId, cancellationToken);
        if (roles == null)
        {
            throw new NotFoundException("Клеймы не найден");
        }
        
        return _mapper.Map<IEnumerable<RoleClaimsDto>>(roles
            .Take(dto.Limit)
            .Skip(dto.Offset));
    }
}