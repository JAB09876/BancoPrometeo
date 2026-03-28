using System.Data;
using BancoPrometeo.Application.Features.Accounts.DTOs;
using BancoPrometeo.Application.Interfaces;
using Dapper;

namespace BancoPrometeo.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AccountRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> OpenAccountAsync(OpenAccountDto dto, string createdBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@CustomerId", dto.CustomerId);
        p.Add("@AccountTypeCode", dto.AccountTypeCode);
        p.Add("@CurrencyCode", dto.CurrencyCode);
        p.Add("@InitialDeposit", dto.InitialDeposit);
        p.Add("@CreatedBy", createdBy);
        p.Add("@AccountId", dbType: DbType.Guid, direction: ParameterDirection.Output);

        await conn.ExecuteAsync("Core.sp_OpenAccount", p, commandType: CommandType.StoredProcedure);
        return p.Get<Guid>("@AccountId");
    }

    public async Task SetAccountStatusAsync(Guid accountId, string newStatus, string? reason, string updatedBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@AccountId", accountId);
        p.Add("@NewStatus", newStatus);
        p.Add("@Reason", reason);
        p.Add("@UpdatedBy", updatedBy);

        await conn.ExecuteAsync("Core.sp_SetAccountStatus", p, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<AccountSummaryDto>> GetAllAccountsAsync()
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<AccountSummaryDto>("SELECT * FROM Core.vw_CustomerAccounts");
    }

    public async Task<IEnumerable<AccountSummaryDto>> GetCustomerAccountsAsync(Guid customerId)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<AccountSummaryDto>(
            "SELECT * FROM Core.vw_CustomerAccounts WHERE CustomerId = @CustomerId",
            new { CustomerId = customerId });
    }

    public async Task<AccountDetailDto?> GetAccountByIdAsync(Guid accountId)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<AccountDetailDto>(
            "SELECT * FROM Core.vw_CustomerAccounts WHERE AccountId = @AccountId",
            new { AccountId = accountId });
    }
}
