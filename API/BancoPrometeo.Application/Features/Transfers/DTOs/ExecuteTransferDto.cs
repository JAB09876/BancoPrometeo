namespace BancoPrometeo.Application.Features.Transfers.DTOs;

public record ExecuteTransferDto(
    Guid SourceAccountId,
    Guid? TargetAccountId,
    string TargetAccountNumber,
    string? TargetBankCode,
    string? TargetHolderName,
    decimal Amount,
    string CurrencyCode,
    string Description,
    string Channel
);
