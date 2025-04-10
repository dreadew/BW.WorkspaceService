namespace WorkspaceService.Domain.DTOs.Identity;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PhotoPath { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}