using System.Data;
using BancoPrometeo.Application.Features.Loans.DTOs;
using BancoPrometeo.Application.Interfaces;
using Dapper;

namespace BancoPrometeo.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public LoanRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateLoanRequestAsync(CreateLoanDto dto, string createdBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@CustomerId", dto.CustomerId);
        p.Add("@Principal", dto.Principal);
        p.Add("@AnnualInterestRate", dto.AnnualInterestRate);
        p.Add("@TermMonths", dto.TermMonths);
        p.Add("@Purpose", dto.Purpose);
        p.Add("@CreatedBy", createdBy);
        p.Add("@LoanId", dbType: DbType.Guid, direction: ParameterDirection.Output);

        await conn.ExecuteAsync("Core.sp_CreateLoanRequest", p, commandType: CommandType.StoredProcedure);
        return p.Get<Guid>("@LoanId");
    }

    public async Task ApproveLoanAsync(Guid loanId, string? notes, string approvedBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@LoanId", loanId);
        p.Add("@Notes", notes);
        p.Add("@ApprovedBy", approvedBy);

        await conn.ExecuteAsync("Core.sp_ApproveLoan", p, commandType: CommandType.StoredProcedure);
    }

    public async Task PayInstallmentAsync(Guid loanId, PayLoanInstallmentDto dto, string paidBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@LoanId", loanId);
        p.Add("@SourceAccountId", dto.SourceAccountId);
        p.Add("@Amount", dto.Amount);
        p.Add("@PaidBy", paidBy);

        await conn.ExecuteAsync("Core.sp_PayLoanInstallment", p, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<LoanSummaryDto>> GetAllLoansAsync(string? status)
    {
        using var conn = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Core.vw_LoanPortfolio";
        if (!string.IsNullOrEmpty(status))
            sql += " WHERE Status = @Status";
        return await conn.QueryAsync<LoanSummaryDto>(sql, new { Status = status });
    }

    public async Task<IEnumerable<LoanSummaryDto>> GetCustomerLoansAsync(Guid customerId)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<LoanSummaryDto>(
            "SELECT * FROM Core.vw_LoanPortfolio WHERE CustomerId = @CustomerId",
            new { CustomerId = customerId });
    }

    public async Task<LoanDetailDto?> GetLoanByIdAsync(Guid loanId)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<LoanDetailDto>(
            "SELECT * FROM Core.vw_LoanPortfolio WHERE LoanId = @LoanId",
            new { LoanId = loanId });
    }

    public async Task<IEnumerable<LoanInstallmentDto>> GetInstallmentsAsync(Guid loanId)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<LoanInstallmentDto>(
            "SELECT * FROM Core.LoanInstallments WHERE LoanId = @LoanId ORDER BY InstallmentNumber",
            new { LoanId = loanId });
    }

    public async Task<IEnumerable<AmortizationRowDto>> GetAmortizationTableAsync(Guid loanId)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<AmortizationRowDto>(
            "SELECT * FROM Core.fn_GetAmortizationTable(@LoanId) OPTION (MAXRECURSION 360)",
            new { LoanId = loanId });
    }

    public async Task<decimal> SimulateMonthlyPaymentAsync(decimal principal, decimal annualRate, int termMonths)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.ExecuteScalarAsync<decimal>(
            "SELECT Core.fn_CalculateMonthlyPayment(@Principal, @AnnualRate, @TermMonths)",
            new { Principal = principal, AnnualRate = annualRate, TermMonths = termMonths });
    }
}
