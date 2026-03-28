namespace BancoPrometeo.Domain.Entities;

public class ServicePayment
{
    public Guid PaymentId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid ServiceProviderId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ConfirmationNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}
