namespace BancoPrometeo.Application.Features.Investments.DTOs;

public record CreateInvestmentDto(
    Guid CustomerId,
    Guid SourceAccountId,
    Guid ProductId,
    decimal Amount,
    bool AutoRenew
);
