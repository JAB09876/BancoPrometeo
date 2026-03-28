using BancoPrometeo.Application.Features.Accounts.DTOs;
using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IAccountRepository _accounts;
    private readonly ICurrentUserService _currentUser;

    public AccountsController(IAccountRepository accounts, ICurrentUserService currentUser)
    {
        _accounts = accounts;
        _currentUser = currentUser;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Cajero")]
    public async Task<IActionResult> OpenAccount([FromBody] OpenAccountDto dto)
    {
        var accountId = await _accounts.OpenAccountAsync(dto, _currentUser.Email!);
        return CreatedAtAction(nameof(GetAccount), new { accountId }, new { accountId });
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Cajero")]
    public async Task<IActionResult> GetAllAccounts()
    {
        var accounts = await _accounts.GetAllAccountsAsync();
        return Ok(accounts);
    }

    [HttpGet("my-accounts")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> GetMyAccounts()
    {
        var accounts = await _accounts.GetCustomerAccountsAsync(_currentUser.CustomerId!.Value);
        return Ok(accounts);
    }

    [HttpGet("{accountId:guid}")]
    public async Task<IActionResult> GetAccount(Guid accountId)
    {
        var account = await _accounts.GetAccountByIdAsync(accountId);
        if (account is null) return NotFound();

        // Clients can only see their own accounts
        if (_currentUser.IsInRole("Cliente") && account.CustomerId != _currentUser.CustomerId)
            return Forbid();

        return Ok(account);
    }

    [HttpPatch("{accountId:guid}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetAccountStatus(Guid accountId, [FromBody] SetAccountStatusDto dto)
    {
        await _accounts.SetAccountStatusAsync(accountId, dto.NewStatus, dto.Reason, _currentUser.Email!);
        return NoContent();
    }
}
