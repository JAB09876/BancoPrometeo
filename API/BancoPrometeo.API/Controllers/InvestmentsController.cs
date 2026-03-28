using BancoPrometeo.Application.Features.Investments.DTOs;
using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/investments")]
[Authorize]
public class InvestmentsController : ControllerBase
{
    private readonly IInvestmentRepository _investments;
    private readonly ICurrentUserService _currentUser;

    public InvestmentsController(IInvestmentRepository investments, ICurrentUserService currentUser)
    {
        _investments = investments;
        _currentUser = currentUser;
    }

    [HttpPost]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> CreateInvestment([FromBody] CreateInvestmentDto dto)
    {
        var investmentId = await _investments.CreateInvestmentAsync(dto, _currentUser.Email!);
        return CreatedAtAction(nameof(GetInvestment), new { investmentId }, new { investmentId });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllInvestments()
    {
        var investments = await _investments.GetAllInvestmentsAsync();
        return Ok(investments);
    }

    [HttpGet("my-investments")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> GetMyInvestments()
    {
        var investments = await _investments.GetCustomerInvestmentsAsync(_currentUser.CustomerId!.Value);
        return Ok(investments);
    }

    [HttpGet("simulate")]
    [AllowAnonymous]
    public async Task<IActionResult> Simulate([FromQuery] decimal principal, [FromQuery] decimal annualRate, [FromQuery] int days)
    {
        var estimatedReturn = await _investments.SimulateReturnAsync(principal, annualRate, days);
        return Ok(new { principal, annualRate, days, estimatedReturn });
    }

    [HttpGet("{investmentId:guid}")]
    public async Task<IActionResult> GetInvestment(Guid investmentId)
    {
        var investment = await _investments.GetInvestmentByIdAsync(investmentId);
        if (investment is null) return NotFound();

        if (_currentUser.IsInRole("Cliente") && investment.CustomerId != _currentUser.CustomerId)
            return Forbid();

        return Ok(investment);
    }
}
