namespace BancoPrometeo.Application.Features.Transactions.DTOs;

public record TransactionQueryDto(
    Guid? AccountId,
    int Page = 1,
    int PageSize = 20,
    DateTime? FromDate = null,
    DateTime? ToDate = null
);
