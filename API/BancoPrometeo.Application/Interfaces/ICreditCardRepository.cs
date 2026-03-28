using BancoPrometeo.Application.Features.CreditCards.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface ICreditCardRepository
{
    Task PayCreditCardAsync(Guid cardId, PayCreditCardDto dto, string paidBy);
}
