using AutoMapper;
using WorkspaceService.Domain.DTOs.File;

namespace WorkspaceService.Application.Mapping;

public class FileProfile : Profile
{
    public FileProfile()
    {
        CreateMap<FileUploadRequest, FileUploadDto>();
    }
}