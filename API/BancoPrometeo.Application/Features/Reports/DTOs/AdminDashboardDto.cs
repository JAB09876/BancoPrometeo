namespace BancoPrometeo.Application.Features.Reports.DTOs;

public class AdminDashboardDto
{
    public int TotalCustomers { get; set; }
    public int ActiveAccounts { get; set; }
    public decimal TotalDeposits { get; set; }
    public decimal TotalLoansOutstanding { get; set; }
    public int ActiveLoans { get; set; }
    public int OverdueLoans { get; set; }
    public decimal TotalInvestments { get; set; }
    public int PendingLoanRequests { get; set; }
    public decimal TodayTransactions { get; set; }
    public DateTime GeneratedAt { get; set; }
}
