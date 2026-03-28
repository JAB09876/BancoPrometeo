namespace BancoPrometeo.Application.Features.Transactions.DTOs;

public record WithdrawDto(Guid AccountId, decimal Amount, string Description, string Channel);
