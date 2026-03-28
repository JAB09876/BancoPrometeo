namespace BancoPrometeo.Application.Features.Auth.DTOs;

public record RegisterUserDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    Guid? CustomerId,
    string Role
);
