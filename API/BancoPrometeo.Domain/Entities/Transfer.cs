namespace BancoPrometeo.Domain.Entities;

public class Transfer
{
    public Guid TransferId { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid? TargetAccountId { get; set; }
    public string TargetAccountNumber { get; set; } = string.Empty;
    public string? TargetBankCode { get; set; }
    public string? TargetHolderName { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TransferType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public Guid? ReversedByTransferId { get; set; }
    public string? ReverseReason { get; set; }
}
