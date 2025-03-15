using AutoMapper;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Excpetions;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Application.Services;

public class WorkspacesService : IWorkspaceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityService _identityService;
    private readonly ILogger<WorkspacesService> _logger;
    private readonly IMapper _mapper;

    public WorkspacesService(IUnitOfWork unitOfWork,
        IIdentityService identityService,
        ILogger<WorkspacesService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _identityService = identityService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateAsync(CreateWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var positionRepository = _unitOfWork.Repository<WorkspacePositions>();
        var roleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaims>();
        var roleRepository = _unitOfWork.Repository<WorkspaceRoles>();
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var workspace = _mapper.Map<Workspaces>(dto);
            workspace.Id = Guid.NewGuid().ToString();
            
            var positions = new List<WorkspacePositions>(2)
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Owner",
                    WorkspaceId = workspace.Id
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    WorkspaceId = workspace.Id
                }
            };

            var roles = new List<WorkspaceRoles>(2)
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    WorkspaceId = workspace.Id,
                    Name = "Admin"
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    WorkspaceId = workspace.Id,
                    Name = "User"
                },
            };
        
            var roleClaims = new List<WorkspaceRoleClaims>()
                {
                    new() 
                    {
                        Id = Guid.NewGuid().ToString(),
                        Value = "Workspace.Create",
                        RoleId = roles[0].Id
                    },
                    new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Value = "Workspace.Edit",
                        RoleId = roles[0].Id
                    },
                    new() 
                    {
                        Id = Guid.NewGuid().ToString(),
                        Value = "Workspace.View",
                        RoleId = roles[0].Id
                    },
                    new() 
                    {
                        Id = Guid.NewGuid().ToString(),
                        Value = "Workspace.View",
                        RoleId = roles[1].Id
                    }
                };
            
            var user = new WorkspaceUsers()
            {
                UserId = dto.UserId,
                PositionId = positions[0].Id,
                RoleId = roles[0].Id,
            };
            workspace.Users.Add(user);

            var mainDirectory = new WorkspaceDirectory()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "home",
                WorkspaceId = workspace.Id
            };
            
            await workspaceRepository.CreateAsync(workspace, cancellationToken);
            await positionRepository.CreateManyAsync(positions, cancellationToken);
            await roleRepository.CreateManyAsync(roles, cancellationToken);
            await roleClaimsRepository.CreateManyAsync(roleClaims, cancellationToken);
            await workspaceDirectoryRepository.CreateAsync(mainDirectory, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task UpdateAsync(UpdateWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspace = await workspaceRepository.GetByIdAsync(dto.Id,
            cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Рабочее пространство не найдено");
        }

        _mapper.Map(dto, workspace);
        
        await workspaceRepository.UpdateAsync(workspace, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<WorkspaceDto> GetByIdAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspace = await workspaceRepository.GetByIdAsync(id,
            cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Рабочее пространство не найдено");
        }
        
        var users = await _identityService.GetFromArrayAsync(
            workspace.Users.Select(x => x.UserId).ToList(), cancellationToken);
        var workspaceDto = _mapper.Map<WorkspaceDto>(workspace);
        foreach (var user in workspaceDto.Users)
        {
            user.User = users.FirstOrDefault(x => x.Id == user.UserId);
        }
        
        return workspaceDto;
    }

    public async Task<IEnumerable<WorkspaceDto>> ListAsync(ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspaces = await workspaceRepository.ListAsync(dto,
            cancellationToken);
        if (workspaces == null)
        {
            throw new NotFoundException("Рабочее пространство не найдено");
        }
        
        var workspacesDto = _mapper.Map<List<WorkspaceDto>>(workspaces);
        foreach (var workspace in workspacesDto)
        {
            var users = await _identityService.GetFromArrayAsync(
                workspace.Users.Select(x => x.UserId).ToList(), cancellationToken);
            foreach (var user in workspace.Users)
            {
                user.User = users.FirstOrDefault(x => x.Id == user.UserId);
            }
        }
        
        return workspacesDto;
    }

    public async Task DeleteAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspace = await workspaceRepository.GetByIdAsync(id,
            cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Рабочие пространство не найдено");
        }

        await workspaceRepository.DeleteAsync(x => x.Id == id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    public async Task InviteUserAsync(InviteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePositions>();
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRoles>();

        var workspace = await workspaceRepository.FindAsync(
            x => x.Id == dto.Id, cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Не удалось найти рабочее пространство");
        }
        
        var defaultPosition = await workspacePositionsRepository.FindAsync(
            x => x.Name == "User" && x.WorkspaceId == workspace.Id, cancellationToken);
        if (defaultPosition == null)
        {
            throw new NotFoundException("Не удалось найти должность пользователя");
        }

        var defaultRole = await workspaceRolesRepository.FindAsync(
            x => x.Name == "User" && x.WorkspaceId == workspace.Id, cancellationToken);
        if (defaultRole == null)
        {
            throw new NotFoundException("Не удалось найти роль пользователя");
        } 
        
        var entity = new WorkspaceUsers()
        {
            UserId = dto.UserId,
            WorkspaceId = workspace.Id,
            PositionId = defaultPosition.Id,
            RoleId = defaultRole.Id
        };
        workspace.Users.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(UpdateUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePositions>();
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRoles>();
        var workspace = await workspaceRepository.FindAsync(
            x => x.Id == dto.WorkspaceId, cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Не удалось найти рабочее пространство");
        }
        
        var position = await workspacePositionsRepository.FindAsync(
            x => x.Id == dto.PositionId, cancellationToken);
        if (position == null && dto.PositionId != null)
        {
            throw new NotFoundException("Не удалось найти должность пользователя");
        }

        var role = await workspaceRolesRepository.FindAsync(
            x => x.Id == dto.RoleId, cancellationToken);
        if (role == null && dto.RoleId != null)
        {
            throw new NotFoundException("Не удалось найти роль пользователя");
        }

        var user = workspace.Users.FirstOrDefault(x => x.UserId == dto.UserId && x.WorkspaceId == workspace.Id);
        if (user == null)
        {
            throw new NotFoundException("Не удалось найти пользователя");
        }
        
        user.PositionId = position?.Id ?? user.PositionId;
        user.RoleId = role?.Id ?? user.RoleId;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(DeleteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspace = await workspaceRepository.FindAsync(x => x.Id == dto.UserId, 
            cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Не удалось найти рабочее пространство");
        }

        var user = workspace.Users.FirstOrDefault(x => x.UserId == dto.UserId);
        if (user == null)
        {
            throw new NotFoundException("Не удалось найти пользователя");
        }
        
        workspace.Users.Remove(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}