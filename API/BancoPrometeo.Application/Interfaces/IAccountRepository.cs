using BancoPrometeo.Application.Features.Accounts.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface IAccountRepository
{
    Task<Guid> OpenAccountAsync(OpenAccountDto dto, string createdBy);
    Task SetAccountStatusAsync(Guid accountId, string newStatus, string? reason, string updatedBy);
    Task<IEnumerable<AccountSummaryDto>> GetAllAccountsAsync();
    Task<IEnumerable<AccountSummaryDto>> GetCustomerAccountsAsync(Guid customerId);
    Task<AccountDetailDto?> GetAccountByIdAsync(Guid accountId);
}
