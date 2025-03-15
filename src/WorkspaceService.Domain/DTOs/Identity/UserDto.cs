namespace WorkspaceService.Domain.DTOs.Identity;

public record class UserDto(string Id,
    string Username,
    string Email,
    string PhoneNumber,
    string PhotoPath,
    DateTime CreatedAt,
    DateTime? ModifiedAt);