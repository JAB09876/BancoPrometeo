namespace BancoPrometeo.Application.Features.Transactions.DTOs;

public record DepositDto(Guid AccountId, decimal Amount, string Description, string Channel);
