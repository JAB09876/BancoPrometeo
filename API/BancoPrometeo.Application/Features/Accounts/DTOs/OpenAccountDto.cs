namespace BancoPrometeo.Application.Features.Accounts.DTOs;

public record OpenAccountDto(
    Guid CustomerId,
    string AccountTypeCode,
    string CurrencyCode,
    decimal InitialDeposit
);
