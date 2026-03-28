using System.Data;
using BancoPrometeo.Application.Features.Investments.DTOs;
using BancoPrometeo.Application.Interfaces;
using Dapper;

namespace BancoPrometeo.Infrastructure.Repositories;

public class InvestmentRepository : IInvestmentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public InvestmentRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateInvestmentAsync(CreateInvestmentDto dto, string createdBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@CustomerId", dto.CustomerId);
        p.Add("@SourceAccountId", dto.SourceAccountId);
        p.Add("@ProductId", dto.ProductId);
        p.Add("@Amount", dto.Amount);
        p.Add("@AutoRenew", dto.AutoRenew);
        p.Add("@CreatedBy", createdBy);
        p.Add("@InvestmentId", dbType: DbType.Guid, direction: ParameterDirection.Output);

        await conn.ExecuteAsync("Core.sp_CreateInvestment", p, commandType: CommandType.StoredProcedure);
        return p.Get<Guid>("@InvestmentId");
    }

    public async Task<IEnumerable<InvestmentSummaryDto>> GetAllInvestmentsAsync()
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<InvestmentSummaryDto>("SELECT * FROM Core.vw_InvestmentPortfolio");
    }

    public async Task<IEnumerable<InvestmentSummaryDto>> GetCustomerInvestmentsAsync(Guid customerId)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<InvestmentSummaryDto>(
            "SELECT * FROM Core.vw_InvestmentPortfolio WHERE CustomerId = @CustomerId",
            new { CustomerId = customerId });
    }

    public async Task<InvestmentDetailDto?> GetInvestmentByIdAsync(Guid investmentId)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<InvestmentDetailDto>(
            "SELECT * FROM Core.vw_InvestmentPortfolio WHERE InvestmentId = @InvestmentId",
            new { InvestmentId = investmentId });
    }

    public async Task<decimal> SimulateReturnAsync(decimal principal, decimal annualRate, int days)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.ExecuteScalarAsync<decimal>(
            "SELECT Core.fn_CalculateInvestmentReturn(@Principal, @AnnualRate, @Days)",
            new { Principal = principal, AnnualRate = annualRate, Days = days });
    }
}
