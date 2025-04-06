using System.Text.Json;
using AutoMapper;
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

public class WorkspacesService : IWorkspaceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityServiceClient _identityService;
    private readonly IKafkaProducerService _kafkaProducerService;
    private readonly ILogger<WorkspacesService> _logger;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;

    public WorkspacesService(IUnitOfWork unitOfWork,
        IIdentityServiceClient identityService,
        IKafkaProducerService kafkaProducerService,
        ILogger<WorkspacesService> logger,
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
            workspace.CreatedBy = dto.UserId;
            
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
        await _claimsService.CheckUserClaim(dto.Id, dto.FromId, "Workspace.Edit", cancellationToken);
        var eventsRepository = _unitOfWork.Repository<Events>();
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspace = await workspaceRepository.GetByIdAsync(dto.Id,
            cancellationToken);
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
                var newEvent = new Events()
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

    public async Task DeleteAsync(DeleteWorkspaceRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _claimsService.CheckUserClaim(dto.WorkspaceId, dto.FromId, "Workspace.Edit", 
            cancellationToken);
        var eventsRepository = _unitOfWork.Repository<Events>();
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspace = await workspaceRepository.GetByIdAsync(dto.WorkspaceId,
            cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException("Рабочее пространство не найдено");
        }
        
        workspace.IsDeleted = true;
        var actualityDto = new WorkspaceChangedActualityDto()
        {
            WorkspaceId = workspace.Id,
            Actuality = workspace.IsDeleted
        };
        var serializedDto = JsonSerializer.Serialize(actualityDto);
        var newEvent = new Events()
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
    
    public async Task InviteUserAsync(InviteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _claimsService.CheckUserClaim(dto.Id, dto.FromId, "Workspace.Edit", cancellationToken);
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
        await _claimsService.CheckUserClaim(dto.WorkspaceId, dto.FromId, "Workspace.Edit", 
            cancellationToken);
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
        await _claimsService.CheckUserClaim(dto.WorkspaceId, dto.FromId, "Workspace.Edit", 
            cancellationToken);
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

    public async Task UploadPictureAsync(string workspaceId,
        FileUploadRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspace = await workspaceRepository.FindAsync(x => x.Id == workspaceId, cancellationToken);
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
        var workspaceRepository = _unitOfWork.Repository<Workspaces>();
        var workspace = await workspaceRepository.FindAsync(x => x.Id == workspaceId);
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
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRoles>();
        var roles = await workspaceRolesRepository
            .FindManyAsync(x => x.WorkspaceId == workspaceId, cancellationToken);
        foreach (var role in roles)
        {
            role.IsDeleted = actuality;
            await workspaceRolesRepository.UpdateAsync(role, cancellationToken);
        }
    }

    private async Task HandleWorkspacePositions(string workspaceId, bool actuality,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePositions>();
        var positions = await workspacePositionsRepository
            .FindManyAsync(x => x.WorkspaceId == workspaceId, cancellationToken);
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
            .FindManyAsync(x => x.WorkspaceId == workspaceId, cancellationToken);
        foreach (var directory in directories)
        {
            directory.IsDeleted = actuality;
            await workspaceDirectoriesRepository.UpdateAsync(directory, cancellationToken);
        }
    }
}