namespace BancoPrometeo.Domain.Entities;

public class Customer
{
    public Guid CustomerId { get; set; }
    public string IdentificationNumber { get; set; } = string.Empty;
    public string IdentificationType { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
