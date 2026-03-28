namespace BancoPrometeo.Application.Features.Accounts.DTOs;

public class AccountDetailDto : AccountSummaryDto
{
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public DateTime? ClosedAt { get; set; }
}
