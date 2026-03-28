namespace BancoPrometeo.Application.Features.CreditCards.DTOs;

public record PayCreditCardDto(Guid SourceAccountId, decimal Amount);
