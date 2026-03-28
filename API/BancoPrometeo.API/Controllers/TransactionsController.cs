using BancoPrometeo.Application.Features.Transactions.DTOs;
using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionRepository _transactions;
    private readonly ICurrentUserService _currentUser;

    public TransactionsController(ITransactionRepository transactions, ICurrentUserService currentUser)
    {
        _transactions = transactions;
        _currentUser = currentUser;
    }

    [HttpPost("deposit")]
    [Authorize(Roles = "Admin,Cajero")]
    public async Task<IActionResult> Deposit([FromBody] DepositDto dto)
    {
        var txnId = await _transactions.DepositAsync(dto, _currentUser.Email!);
        return Ok(new { txnId });
    }

    [HttpPost("withdraw")]
    [Authorize(Roles = "Admin,Cajero")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawDto dto)
    {
        var txnId = await _transactions.WithdrawAsync(dto, _currentUser.Email!);
        return Ok(new { txnId });
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromQuery] TransactionQueryDto query)
    {
        // Clients only see their own transactions
        Guid? customerId = _currentUser.IsInRole("Cliente") ? _currentUser.CustomerId : null;
        var result = await _transactions.GetTransactionsAsync(query, customerId);
        return Ok(result);
    }
}
