using Common.Base.DTO;

namespace WorkspaceService.Domain.DTOs;

public class ListRequestWithUserId : ListRequest
{
    public string UserId { get; set; }
}