namespace BancoPrometeo.Application.Features.Loans.DTOs;

public class LoanDetailDto : LoanSummaryDto
{
    public string? Notes { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TotalInstallments { get; set; }
    public int PaidInstallments { get; set; }
    public int OverdueInstallments { get; set; }
}
