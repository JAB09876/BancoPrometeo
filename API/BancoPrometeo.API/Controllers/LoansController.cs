using BancoPrometeo.Application.Features.Loans.DTOs;
using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/loans")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly ILoanRepository _loans;
    private readonly ICurrentUserService _currentUser;

    public LoansController(ILoanRepository loans, ICurrentUserService currentUser)
    {
        _loans = loans;
        _currentUser = currentUser;
    }

    [HttpPost]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> CreateLoanRequest([FromBody] CreateLoanDto dto)
    {
        var loanId = await _loans.CreateLoanRequestAsync(dto, _currentUser.Email!);
        return CreatedAtAction(nameof(GetLoan), new { loanId }, new { loanId });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllLoans([FromQuery] string? status)
    {
        var loans = await _loans.GetAllLoansAsync(status);
        return Ok(loans);
    }

    [HttpGet("my-loans")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> GetMyLoans()
    {
        var loans = await _loans.GetCustomerLoansAsync(_currentUser.CustomerId!.Value);
        return Ok(loans);
    }

    [HttpGet("simulate")]
    public async Task<IActionResult> Simulate([FromQuery] decimal principal, [FromQuery] decimal annualRate, [FromQuery] int termMonths)
    {
        var monthlyPayment = await _loans.SimulateMonthlyPaymentAsync(principal, annualRate, termMonths);
        return Ok(new { principal, annualRate, termMonths, monthlyPayment });
    }

    [HttpGet("{loanId:guid}")]
    public async Task<IActionResult> GetLoan(Guid loanId)
    {
        var loan = await _loans.GetLoanByIdAsync(loanId);
        if (loan is null) return NotFound();

        if (_currentUser.IsInRole("Cliente") && loan.CustomerId != _currentUser.CustomerId)
            return Forbid();

        return Ok(loan);
    }

    [HttpGet("{loanId:guid}/installments")]
    public async Task<IActionResult> GetInstallments(Guid loanId)
    {
        var installments = await _loans.GetInstallmentsAsync(loanId);
        return Ok(installments);
    }

    [HttpGet("{loanId:guid}/amortization")]
    [Authorize(Roles = "Cliente,Admin")]
    public async Task<IActionResult> GetAmortization(Guid loanId)
    {
        var table = await _loans.GetAmortizationTableAsync(loanId);
        return Ok(table);
    }

    [HttpPost("{loanId:guid}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveLoan(Guid loanId, [FromBody] ApproveLoanDto dto)
    {
        await _loans.ApproveLoanAsync(loanId, dto.Notes, _currentUser.Email!);
        return NoContent();
    }

    [HttpPost("{loanId:guid}/pay")]
    [Authorize(Roles = "Cliente,Admin,Cajero")]
    public async Task<IActionResult> PayInstallment(Guid loanId, [FromBody] PayLoanInstallmentDto dto)
    {
        await _loans.PayInstallmentAsync(loanId, dto, _currentUser.Email!);
        return NoContent();
    }
}
