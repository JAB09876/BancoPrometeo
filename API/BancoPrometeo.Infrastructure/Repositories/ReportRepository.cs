using System.Data;
using BancoPrometeo.Application.Features.Reports.DTOs;
using BancoPrometeo.Application.Interfaces;
using Dapper;

namespace BancoPrometeo.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ReportRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<DailyClosingResultDto> ExecuteDailyClosingAsync(DateOnly? closingDate, string executedBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@ClosingDate", closingDate?.ToDateTime(TimeOnly.MinValue));
        p.Add("@ExecutedBy", executedBy);

        var result = await conn.QueryFirstOrDefaultAsync<DailyClosingResultDto>(
            "Core.sp_DailyClosing", p, commandType: CommandType.StoredProcedure);

        return result ?? new DailyClosingResultDto { Status = "COMPLETED", ClosingDate = closingDate ?? DateOnly.FromDateTime(DateTime.Today) };
    }

    public async Task<AdminDashboardDto> GetAdminDashboardAsync()
    {
        using var conn = _connectionFactory.CreateConnection();
        var result = await conn.QueryFirstOrDefaultAsync<AdminDashboardDto>("SELECT * FROM Core.vw_AdminDashboard");
        return result ?? new AdminDashboardDto { GeneratedAt = DateTime.UtcNow };
    }
}
