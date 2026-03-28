namespace BancoPrometeo.Application.Features.Auth.DTOs;

public class UserDto
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public IEnumerable<string> Roles { get; set; } = [];
}
