namespace BancoPrometeo.Domain.Entities;

public class Investment
{
    public Guid InvestmentId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Principal { get; set; }
    public decimal AnnualRate { get; set; }
    public int TermDays { get; set; }
    public bool AutoRenew { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public decimal? AccruedInterest { get; set; }
    public DateTime CreatedAt { get; set; }
}
