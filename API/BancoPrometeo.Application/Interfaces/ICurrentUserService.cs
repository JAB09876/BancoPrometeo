namespace BancoPrometeo.Application.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    Guid? CustomerId { get; }
    IEnumerable<string> Roles { get; }
    bool IsInRole(string role);
}
