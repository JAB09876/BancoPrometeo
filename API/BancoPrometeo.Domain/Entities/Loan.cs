using BancoPrometeo.Domain.Enums;

namespace BancoPrometeo.Domain.Entities;

public class Loan
{
    public Guid LoanId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Principal { get; set; }
    public decimal AnnualInterestRate { get; set; }
    public int TermMonths { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal OutstandingBalance { get; set; }
    public LoanStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
