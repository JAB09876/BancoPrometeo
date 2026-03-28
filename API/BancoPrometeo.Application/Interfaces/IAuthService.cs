using BancoPrometeo.Application.Features.Auth.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> LoginAsync(LoginDto dto);
    Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
    Task RevokeTokenAsync(string refreshToken);
    Task<UserDto> RegisterUserAsync(RegisterUserDto dto);
    Task ChangePasswordAsync(string userId, ChangePasswordDto dto);
}
