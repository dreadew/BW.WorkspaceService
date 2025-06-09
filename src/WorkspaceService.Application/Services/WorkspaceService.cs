using System.Text.Json;
using AutoMapper;
using Common.Base.Context;
using Common.Base.DTO;
using Common.Base.DTO.File;
using Common.Base.Exceptions;
using Common.Base.Extensions;
using Common.Base.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.Messaging;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Enums;
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
            workspace.CreatedBy = Guid.Parse(CurrentUserContext.CurrentUserId);
            
            var positions = new List<WorkspacePosition>(2)
            {
                new()
                {
                    Name = "Owner",
                    IsDeleted = false
                },
                new()
                {
                    Name = "User",
                    IsDeleted = false
                }
            };
            workspace.Positions.AddRange(positions);
            var ownerPosition = positions.First(x => x.Name == "Owner");
            
            var roles = new List<WorkspaceRole>(2)
            {
                new()
                {
                    Name = "Admin",
                    IsDeleted = false
                },
                new()
                {
                    Name = "User",
                    IsDeleted = false
                },
            };
            workspace.Roles.AddRange(roles);
            var adminRole = roles.First(x => x.Name == "Admin");
            var userRole = roles.First(x => x.Name == "User");
        
            var adminRoleClaims = new List<WorkspaceRoleClaim>()
                {
                    new() 
                    {
                        Value = "Workspace.Create",
                    },
                    new()
                    {
                        Value = "Workspace.Edit",
                    },
                    new() 
                    {
                        Value = "Workspace.View",
                    },
                    new()
                    {
                        Value = "Project.Create",
                    },
                };
            adminRole.Claims.AddRange(adminRoleClaims);
            
            var userRoleClaims = new List<WorkspaceRoleClaim>()
            {
                new() 
                {
                    Value = "Workspace.Create",
                },
                new()
                {
                    Value = "Workspace.Edit",
                },
                new() 
                {
                    Value = "Workspace.View",
                },
                new()
                {
                    Value = "Project.Create",
                },
            };
            userRole.Claims.AddRange(userRoleClaims);
            
            var user = new WorkspaceUser()
            {
                UserId = Guid.Parse(CurrentUserContext.CurrentUserId),
                Position = ownerPosition,
                Role = adminRole,
            };
            workspace.Users.Add(user);

            var mainDirectory = new WorkspaceDirectory()
            {
                Name = "home",
                IsDeleted = false
            };
            workspace.Directories.Add(mainDirectory);
            
            await workspaceRepository.CreateAsync(workspace, cancellationToken);
            await positionRepository.CreateManyAsync(positions, cancellationToken);
            await roleRepository.CreateManyAsync(roles, cancellationToken);
            await roleClaimsRepository.CreateManyAsync(adminRoleClaims, cancellationToken);
            await roleClaimsRepository.CreateManyAsync(userRoleClaims, cancellationToken);
            await workspaceDirectoryRepository.CreateAsync(mainDirectory, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
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
        await _claimsService.CheckUserClaim(dto.Id, CurrentUserContext.CurrentUserId, "Workspace.Edit", cancellationToken);
        var eventsRepository = _unitOfWork.Repository<Event>();
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }

        _mapper.Map(dto, workspace);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            workspaceRepository.Update(workspace, cancellationToken);
            if (dto.IsDeleted != null)
            {
                var actualityDto = new WorkspaceChangedActualityDto
                {
                    WorkspaceId = workspace.Id.ToString(),
                    Actuality = workspace.IsDeleted
                };
                var serializedDto = JsonSerializer.Serialize(actualityDto);
                var newEvent = new Event()
                {
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

    public async Task<WorkspaceDto> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }
        
        var users = await _identityService.GetFromArrayAsync(
            workspace.Users.Select(x => x.UserId.ToString()).ToList(), cancellationToken);
        var workspaceDto = _mapper.Map<WorkspaceDto>(workspace);
        foreach (var user in workspaceDto.Users)
        {
            user.User = users.FirstOrDefault(x => x.Id == user.Id);
        }
        
        return workspaceDto;
    }

    public async Task<IEnumerable<WorkspaceDto>> ListAsync(ListRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspaces = await workspaceRepository
            .GetAll()
            .WhereIf(!dto.IncludeDeleted, d => !d.IsDeleted)
            .Paging(dto)
            .ToListAsync(cancellationToken);
        if (workspaces == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }
        
        var workspacesDto = _mapper.Map<List<WorkspaceDto>>(workspaces);
        foreach (var workspace in workspacesDto)
        {
            var users = await _identityService.GetFromArrayAsync(
                workspace.Users.Select(x => x.Id).ToList(), cancellationToken);
            foreach (var user in workspace.Users)
            {
                user.User = users.FirstOrDefault(x => x.Id == user.Id);
            }
        }
        
        return workspacesDto;
    }

    public async Task DeleteAsync(DeleteWorkspaceRequest dto,
        CancellationToken cancellationToken = default) => await UpdateActualityInternal
        (dto.Id, CurrentUserContext.CurrentUserId, true, cancellationToken);
    
    public async Task RestoreAsync(RestoreWorkspaceRequest dto,
        CancellationToken cancellationToken = default) => await UpdateActualityInternal
        (dto.Id, CurrentUserContext.CurrentUserId, false, cancellationToken);
    
    public async Task InviteUserAsync(InviteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _claimsService.CheckUserClaim(dto.Id, CurrentUserContext.CurrentUserId, "Workspace.Edit", cancellationToken);
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();

        var workspace = await workspaceRepository
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }
        
        var defaultPosition = await workspacePositionsRepository
            .FindMany(x => x.Name == "User" && x.WorkspaceId == workspace.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (defaultPosition == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.PositionNotFound);
        }

        var defaultRole = await workspaceRolesRepository
            .FindMany(x => x.Name == "User" && x.WorkspaceId == workspace.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (defaultRole == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.UserNotFound);
        } 
        
        var entity = new WorkspaceUser()
        {
            UserId = Guid.Parse(dto.UserId),
            Position = defaultPosition,
            Role = defaultRole,
        };
        workspace.Users.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(UpdateUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _claimsService.CheckUserClaim(dto.Id, CurrentUserContext.CurrentUserId, "Workspace.Edit", 
            cancellationToken);
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == Guid.Parse(dto.Id))
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }
        
        var position = await workspacePositionsRepository
            .FindMany(x => x.Id == Guid.Parse(dto.PositionId ?? ""))
            .FirstOrDefaultAsync(cancellationToken);
        if (position == null && dto.PositionId != null)
        {
            throw new NotFoundException(ExceptionResourceKeys.PositionNotFound);
        }

        var role = await workspaceRolesRepository
            .FindMany(x => x.Id == Guid.Parse(dto.RoleId ?? ""))
            .FirstOrDefaultAsync(cancellationToken);
        if (role == null && dto.RoleId != null)
        {
            throw new NotFoundException(ExceptionResourceKeys.RoleNotFound);
        }

        var user = workspace.Users
            .FirstOrDefault(x => x.UserId == Guid.Parse(dto.UserId)
                                 && x.WorkspaceId == workspace.Id);
        if (user == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.UserNotFound);
        }
        
        user.Position = position ?? user.Position;
        user.Role = role ?? user.Role;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(DeleteUserRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _claimsService.CheckUserClaim(dto.Id, CurrentUserContext.CurrentUserId, "Workspace.Edit", 
            cancellationToken);
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == Guid.Parse(dto.UserId))
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }

        var user = workspace.Users.FirstOrDefault(x => x.UserId == Guid.Parse(dto.UserId));
        if (user == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.UserNotFound);
        }
        
        workspace.Users.Remove(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UploadPictureAsync(Guid workspaceId,
        FileUploadRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == workspaceId)
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }

        if (!workspace.Users.Any(x => x.UserId == dto.FromId))
        {
            throw new ServiceException(Common.Base.Constants.ExceptionResourceKeys.NoAccess, true);
        }

        var uploadPath = new List<string>() { "workspace", $"{workspace.Id}", "photo" };
        var uploadDto = _mapper.Map<FileUploadDto>(dto);
        uploadDto.Paths = uploadPath;
        
        var uploadedPath = await _fileService.UploadFileAsync(uploadDto, cancellationToken);
        workspace.PicturePath = uploadedPath.FilePath;
        workspaceRepository.Update(workspace, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePictureAsync(Guid workspaceId,
        FileDeleteRequest dto,
        CancellationToken cancellationToken = default)
    {
        var workspaceRepository = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepository
            .FindMany(x => x.Id == workspaceId)
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }

        if (!workspace.Users.Any(x => x.UserId == Guid.Parse(dto.FromId)))
        {
            throw new ServiceException("У вас нет прав", true);
        }

        if (workspace.PicturePath == null)
        {
            throw new ServiceException(ExceptionResourceKeys.NoPhoto, true);
        }

        var deleteDto = new FileDeleteDto()
        {
            FileName = workspace.PicturePath
        };
        
        await _fileService.DeleteFileAsync(deleteDto, cancellationToken);
        workspace.PicturePath = null;
        workspaceRepository.Update(workspace, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task HandleWorkspaceRoles(Guid workspaceId, bool actuality, CancellationToken cancellationToken = default)
    {
        var workspaceRolesRepository = _unitOfWork.Repository<WorkspaceRole>();
        var roles = await workspaceRolesRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
        foreach (var role in roles)
        {
            role.IsDeleted = actuality;
            workspaceRolesRepository.Update(role, cancellationToken);
        }
    }

    private async Task HandleWorkspacePositions(Guid workspaceId, bool actuality,
        CancellationToken cancellationToken = default)
    {
        var workspacePositionsRepository = _unitOfWork.Repository<WorkspacePosition>();
        var positions = await workspacePositionsRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
        foreach (var position in positions)
        {
            position.IsDeleted = actuality;
            workspacePositionsRepository.Update(position, cancellationToken);
        }
    }

    private async Task HandleWorkspaceDirectories(Guid workspaceId, bool actuality,
        CancellationToken cancellationToken = default)
    {
        var workspaceDirectoriesRepository = _unitOfWork.Repository<WorkspaceDirectory>();
        var directories = await  workspaceDirectoriesRepository
            .FindMany(x => x.WorkspaceId == workspaceId)
            .ToListAsync(cancellationToken);
        foreach (var directory in directories)
        {
            directory.IsDeleted = actuality;
            workspaceDirectoriesRepository.Update(directory, cancellationToken);
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
            .FindMany(x => x.Id == Guid.Parse(id))
            .FirstOrDefaultAsync(cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(ExceptionResourceKeys.WorkspaceNotFound);
        }
        
        workspace.IsDeleted = isDeleted;
        var actualityDto = new WorkspaceChangedActualityDto()
        {
            WorkspaceId = workspace.Id.ToString(),
            Actuality = workspace.IsDeleted
        };
        var serializedDto = JsonSerializer.Serialize(actualityDto);
        var newEvent = new Event()
        {
            EventType = KafkaTopic.WorkspaceChangedActuality,
            Payload = serializedDto,
            IsSent = false
        };
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            //await workspaceRepository.DeleteAsync(x => x.Id == id, cancellationToken);
            await eventsRepository.CreateAsync(newEvent, cancellationToken);
            workspaceRepository.Update(workspace, cancellationToken);
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