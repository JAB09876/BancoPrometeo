namespace BancoPrometeo.Application.Features.Loans.DTOs;

public class AmortizationRowDto
{
    public int Period { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Payment { get; set; }
    public decimal Principal { get; set; }
    public decimal Interest { get; set; }
    public decimal Balance { get; set; }
}
