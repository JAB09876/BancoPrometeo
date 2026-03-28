using System.Security.Claims;
using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BancoPrometeo.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? Email => User?.FindFirstValue(ClaimTypes.Email);

    public Guid? CustomerId
    {
        get
        {
            var value = User?.FindFirstValue("customerId");
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public IEnumerable<string> Roles => User?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? [];

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}
