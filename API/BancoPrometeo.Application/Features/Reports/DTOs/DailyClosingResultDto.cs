namespace BancoPrometeo.Application.Features.Reports.DTOs;

public class DailyClosingResultDto
{
    public DateOnly ClosingDate { get; set; }
    public int AccountsProcessed { get; set; }
    public decimal TotalDeposits { get; set; }
    public decimal TotalWithdrawals { get; set; }
    public decimal TotalTransfers { get; set; }
    public string Status { get; set; } = string.Empty;
}
