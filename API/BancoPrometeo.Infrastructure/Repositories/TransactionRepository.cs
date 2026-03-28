using System.Data;
using BancoPrometeo.Application.Common;
using BancoPrometeo.Application.Features.Transactions.DTOs;
using BancoPrometeo.Application.Interfaces;
using Dapper;

namespace BancoPrometeo.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TransactionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> DepositAsync(DepositDto dto, string createdBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@AccountId", dto.AccountId);
        p.Add("@Amount", dto.Amount);
        p.Add("@Description", dto.Description);
        p.Add("@Channel", dto.Channel);
        p.Add("@CreatedBy", createdBy);
        p.Add("@TxnId", dbType: DbType.Guid, direction: ParameterDirection.Output);

        await conn.ExecuteAsync("Core.sp_Deposit", p, commandType: CommandType.StoredProcedure);
        return p.Get<Guid>("@TxnId");
    }

    public async Task<Guid> WithdrawAsync(WithdrawDto dto, string createdBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@AccountId", dto.AccountId);
        p.Add("@Amount", dto.Amount);
        p.Add("@Description", dto.Description);
        p.Add("@Channel", dto.Channel);
        p.Add("@CreatedBy", createdBy);
        p.Add("@TxnId", dbType: DbType.Guid, direction: ParameterDirection.Output);

        await conn.ExecuteAsync("Core.sp_Withdraw", p, commandType: CommandType.StoredProcedure);
        return p.Get<Guid>("@TxnId");
    }

    public async Task<PagedResult<TransactionDetailDto>> GetTransactionsAsync(TransactionQueryDto query, Guid? customerId)
    {
        using var conn = _connectionFactory.CreateConnection();

        var where = new List<string>();
        var parameters = new DynamicParameters();

        if (query.AccountId.HasValue)
        {
            where.Add("AccountId = @AccountId");
            parameters.Add("@AccountId", query.AccountId.Value);
        }
        if (customerId.HasValue)
        {
            where.Add("CustomerId = @CustomerId");
            parameters.Add("@CustomerId", customerId.Value);
        }
        if (query.FromDate.HasValue)
        {
            where.Add("CreatedAt >= @FromDate");
            parameters.Add("@FromDate", query.FromDate.Value);
        }
        if (query.ToDate.HasValue)
        {
            where.Add("CreatedAt <= @ToDate");
            parameters.Add("@ToDate", query.ToDate.Value);
        }

        var whereClause = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
        var offset = (query.Page - 1) * query.PageSize;

        parameters.Add("@Offset", offset);
        parameters.Add("@PageSize", query.PageSize);

        var countSql = $"SELECT COUNT(*) FROM Core.vw_TransactionDetail {whereClause}";
        var dataSql = $"SELECT * FROM Core.vw_TransactionDetail {whereClause} ORDER BY CreatedAt DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        var total = await conn.ExecuteScalarAsync<int>(countSql, parameters);
        var items = await conn.QueryAsync<TransactionDetailDto>(dataSql, parameters);

        return new PagedResult<TransactionDetailDto>
        {
            Items = items,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }
}
