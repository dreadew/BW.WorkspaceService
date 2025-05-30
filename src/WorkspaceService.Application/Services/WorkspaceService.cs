using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.File;
using WorkspaceService.Domain.DTOs.Messaging;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Enums;
using WorkspaceService.Domain.Exceptions;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Application.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityServiceClient _identityService;
    private readonly IKafkaProducerService _kafkaProducerService;
    private readonly ILogger<WorkspaceService> _logger;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;

    public WorkspaceService(IUnitOfWork unitOfWork,
        IIdentityServiceClient identityService,
        IKafkaProducerService kafkaProducerService,
        ILogger<WorkspaceService> logger,
        IFileService fileService,
        IMapper mapper,
        IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _identityService = identityService;
        _kafkaProducerService = kafkaProducerService;
        _logger = logger;
        _fileService = fileService;
        _mapper = mapper;
        _claimsService = claimsService;
    }

    public async Task CreateAsync(CreateWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var positionRepository = _unitOfWork.Repository<WorkspacePosition>();
        var roleClaimsRepository = _unitOfWork.Repository<WorkspaceRoleClaim>();
        var roleRepository = _unitOfWork.Repository<WorkspaceRole>();
        var workspaceDirectoryRepository = _unitOfWork.Repository<WorkspaceDirectory>();

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var workspace = _mapper.Map<Workspace>(dto);
            workspace.Id = Guid.NewGuid().ToString();
            workspace.CreatedBy = dto.UserId;
            
            var positions = new List<WorkspacePosition>(2)
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

            var roles = new List<WorkspaceRole>(2)
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
        
            var roleClaims = new List<WorkspaceRoleClaim>()
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
                        Value = "Project.Create",
                        RoleId = roles[0].Id
                    },
                    new() 
                    {
                        Id = Guid.NewGuid().ToString(),
                        Value = "Workspace.View",
                        RoleId = roles[1].Id
                    }
                };
            
            var user = new WorkspaceUser()
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
        await _claimsService.CheckUserClaim(dto.Id, dto.FromId, "Workspace.Edit", cancellationToken);
        var eventsRepository = _unitOfWork.Repository<Event>();
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == dto.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Рабочее пространство не найдено");
        }

        _mapper.Map(dto, workspace);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await workspaceRepository.UpdateAsync(workspace, cancellationToken);
            if (dto.IsDeleted != null)
            {
                var actualityDto = new WorkspaceChangedActualityDto
                {
                    WorkspaceId = workspace.Id,
                    Actuality = workspace.IsDeleted
                };
                var serializedDto = JsonSerializer.Serialize(actualityDto);
                var newEvent = new Event()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = KafkaTopic.WorkspaceChangedActuality,
                    Payload = serializedDto,
                    IsSent = false
                };

                await eventsRepository.CreateAsync(newEvent, cancellationToken);
                await HandleWorkspaceRoles(workspace.Id, workspace.IsDeleted, cancellationToken);
                await HandleWorkspacePositions(workspace.Id, workspace.IsDeleted, cancellationToken);
                await HandleWorkspaceDirectories(workspace.Id, workspace.IsDeleted, cancellationToken);
                //await _kafkaProducerService.PublishAsync(KafkaConstants.WorkspaceChangedActualityTopic, actualityDto);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        }
    }

    public async Task<WorkspaceDto> GetByIdAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == id)
            .Include(x => x.Roles)
                .ThenInclude(x => x.RoleClaims)
            .Include(x => x.Positions)
            .Include(x => x.Users)
            .FirstOrDefaultAsync(cancellationToken);
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
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspaces = await workspaceRepository
            .Paging(dto)
            .Include(x => x.Roles)
                .ThenInclude(x => x.RoleClaims)
            .Include(x => x.Positions)
            .Include(x => x.Users)
            .ToListAsync(cancellationToken);
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

    public async Task DeleteAsync(DeleteWorkspaceRequest dto,
        CancellationToken cancellationToken = default) => await UpdateActualityInternal
        (dto.WorkspaceId, dto.FromId, false, cancellationToken);
    
    public async Task RestoreAsync(RestoreWorkspaceRequest dto,
        CancellationToken cancellationToken = default) => await UpdateActualityInternal
        (dto.WorkspaceId, dto.FromId, true, cancellationToken);
    
    public async Task InviteUserAsync(InviteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _claimsService.CheckUserClaim(dto.Id, dto.FromId, "Workspace.Edit", cancellationToken);
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();

        var workspace = await workspaceRepository
            .FindMany(x => x.Id == dto.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Не удалось найти рабочее пространство");
        }
        
        var defaultPosition = await workspacePositionsRepository
            .FindMany(x => x.Name == "User" && x.WorkspaceId == workspace.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (defaultPosition == null)
        {
            throw new NotFoundException("Не удалось найти должность пользователя");
        }

        var defaultRole = await workspaceRolesRepository
            .FindMany(x => x.Name == "User" && x.WorkspaceId == workspace.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (defaultRole == null)
        {
            throw new NotFoundException("Не удалось найти роль пользователя");
        } 
        
        var entity = new WorkspaceUser()
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
        await _claimsService.CheckUserClaim(dto.WorkspaceId, dto.FromId, "Workspace.Edit", 
            cancellationToken);
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == dto.WorkspaceId)
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Не удалось найти рабочее пространство");
        }
        
        var position = await workspacePositionsRepository
            .FindMany(x => x.Id == dto.PositionId)
            .FirstOrDefaultAsync(cancellationToken);
        if (position == null && dto.PositionId != null)
        {
            throw new NotFoundException("Не удалось найти должность пользователя");
        }

        var role = await workspaceRolesRepository
            .FindMany(x => x.Id == dto.RoleId)
            .FirstOrDefaultAsync(cancellationToken);
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
        await _claimsService.CheckUserClaim(dto.WorkspaceId, dto.FromId, "Workspace.Edit", 
            cancellationToken);
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == dto.UserId)
            .FirstOrDefaultAsync(cancellationToken);
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

    public async Task UploadPictureAsync(string workspaceId,
        FileUploadRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == workspaceId)
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Не удалось найти рабочее пространство");
        }

        if (!workspace.Users.Any(x => x.UserId == dto.FromId))
        {
            throw new ServiceException("У вас нет доступа", true);
        }

        var uploadPath = new List<string>() { "workspace", $"{workspace.Id}", "photo" };
        var uploadDto = _mapper.Map<FileUploadDto>(dto);
        uploadDto.Paths = uploadPath;
        
        var uploadedPath = await _fileService.UploadFileAsync(uploadDto, cancellationToken);
        workspace.PicturePath = uploadedPath.FilePath;
        await workspaceRepository.UpdateAsync(workspace, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePictureAsync(string workspaceId,
        FileDeleteRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == workspaceId)
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Рабочее пространство не найдено");
        }

        if (!workspace.Users.Any(x => x.UserId == dto.FromId))
        {
            throw new ServiceException("У вас нет прав", true);
        }

        if (workspace.PicturePath == null)
        {
            throw new ServiceException("У этого рабочего пространства не установлена фотография", true);
        }

        var deleteDto = new FileDeleteDto()
        {
            FileName = workspace.PicturePath
        };
        
        await _fileService.DeleteFileAsync(deleteDto, cancellationToken);
        workspace.PicturePath = null;
        await workspaceRepository.UpdateAsync(workspace, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task HandleWorkspaceRoles(string workspaceId, bool actuality, CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var roles = await workspaceRolesRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
        foreach (var role in roles)
        {
            role.IsDeleted = actuality;
            await workspaceRolesRepository.UpdateAsync(role, cancellationToken);
        }
    }

    private async Task HandleWorkspacePositions(string workspaceId, bool actuality,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var positions = await workspacePositionsRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
        foreach (var position in positions)
        {
            position.IsDeleted = actuality;
            await workspacePositionsRepository.UpdateAsync(position, cancellationToken);
        }
    }

    private async Task HandleWorkspaceDirectories(string workspaceId, bool actuality,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoriesRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directories = await  workspaceDirectoriesRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
        foreach (var directory in directories)
        {
            directory.IsDeleted = actuality;
            await workspaceDirectoriesRepository.UpdateAsync(directory, cancellationToken);
        }
    }

    private async Task UpdateActualityInternal(string id, string fromId, bool isDeleted,
        CancellationToken cancellationToken = default)
    {
        await _claimsService.CheckUserClaim(id, fromId, "Workspace.Edit", 
            cancellationToken);
        var eventsRepository = _unitOfWork.Repository<Event>();
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Рабочее пространство не найдено");
        }
        
        workspace.IsDeleted = isDeleted;
        var actualityDto = new WorkspaceChangedActualityDto()
        {
            WorkspaceId = workspace.Id,
            Actuality = workspace.IsDeleted
        };
        var serializedDto = JsonSerializer.Serialize(actualityDto);
        var newEvent = new Event()
        {
            Id = Guid.NewGuid().ToString(),
            EventType = KafkaTopic.WorkspaceChangedActuality,
            Payload = serializedDto,
            IsSent = false
        };
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            //await workspaceRepository.DeleteAsync(x => x.Id == id, cancellationToken);
            await eventsRepository.CreateAsync(newEvent, cancellationToken);
            await workspaceRepository.UpdateAsync(workspace, cancellationToken);
            await HandleWorkspaceRoles(workspace.Id, workspace.IsDeleted, cancellationToken);
            await HandleWorkspacePositions(workspace.Id, workspace.IsDeleted, cancellationToken);
            await HandleWorkspaceDirectories(workspace.Id, workspace.IsDeleted, cancellationToken);
            //await _kafkaProducerService.PublishAsync(KafkaConstants.WorkspaceChangedActualityTopic, actualityDto);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        } 
    }
}