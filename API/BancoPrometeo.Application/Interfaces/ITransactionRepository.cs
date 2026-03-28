using BancoPrometeo.Application.Common;
using BancoPrometeo.Application.Features.Transactions.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface ITransactionRepository
{
    Task<Guid> DepositAsync(DepositDto dto, string createdBy);
    Task<Guid> WithdrawAsync(WithdrawDto dto, string createdBy);
    Task<PagedResult<TransactionDetailDto>> GetTransactionsAsync(TransactionQueryDto query, Guid? customerId);
}
