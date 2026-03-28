using BancoPrometeo.Application.Features.CreditCards.DTOs;
using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/credit-cards")]
[Authorize]
public class CreditCardsController : ControllerBase
{
    private readonly ICreditCardRepository _creditCards;
    private readonly ICurrentUserService _currentUser;

    public CreditCardsController(ICreditCardRepository creditCards, ICurrentUserService currentUser)
    {
        _creditCards = creditCards;
        _currentUser = currentUser;
    }

    [HttpPost("{cardId:guid}/pay")]
    [Authorize(Roles = "Cliente,Admin,Cajero")]
    public async Task<IActionResult> PayCreditCard(Guid cardId, [FromBody] PayCreditCardDto dto)
    {
        await _creditCards.PayCreditCardAsync(cardId, dto, _currentUser.Email!);
        return NoContent();
    }
}
