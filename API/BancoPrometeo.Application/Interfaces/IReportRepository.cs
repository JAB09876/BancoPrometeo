using BancoPrometeo.Application.Features.Reports.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface IReportRepository
{
    Task<DailyClosingResultDto> ExecuteDailyClosingAsync(DateOnly? closingDate, string executedBy);
    Task<AdminDashboardDto> GetAdminDashboardAsync();
}
