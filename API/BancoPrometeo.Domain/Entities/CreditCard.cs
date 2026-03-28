namespace BancoPrometeo.Domain.Entities;

public class CreditCard
{
    public Guid CardId { get; set; }
    public Guid CustomerId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string CardType { get; set; } = string.Empty;
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal AvailableCredit { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateOnly ExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
