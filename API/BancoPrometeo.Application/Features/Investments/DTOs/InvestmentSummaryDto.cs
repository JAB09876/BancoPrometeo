namespace BancoPrometeo.Application.Features.Investments.DTOs;

public class InvestmentSummaryDto
{
    public Guid InvestmentId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Principal { get; set; }
    public decimal AnnualRate { get; set; }
    public int TermDays { get; set; }
    public bool AutoRenew { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public decimal? AccruedInterest { get; set; }
}
