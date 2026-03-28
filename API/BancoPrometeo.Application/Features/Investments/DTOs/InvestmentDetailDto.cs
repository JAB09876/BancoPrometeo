namespace BancoPrometeo.Application.Features.Investments.DTOs;

public class InvestmentDetailDto : InvestmentSummaryDto
{
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
