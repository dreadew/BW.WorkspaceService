using AutoMapper;
using Common.Base.DTO.File;

namespace WorkspaceService.Application.Mapping;

public class FileProfile : Profile
{
    public FileProfile()
    {
        CreateMap<FileUploadRequest, FileUploadDto>();
    }
}