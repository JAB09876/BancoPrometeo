using BancoPrometeo.Application.Features.Transfers.DTOs;
using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/transfers")]
[Authorize]
public class TransfersController : ControllerBase
{
    private readonly ITransferRepository _transfers;
    private readonly ICurrentUserService _currentUser;

    public TransfersController(ITransferRepository transfers, ICurrentUserService currentUser)
    {
        _transfers = transfers;
        _currentUser = currentUser;
    }

    [HttpPost]
    [Authorize(Roles = "Cliente,Admin,Cajero")]
    public async Task<IActionResult> ExecuteTransfer([FromBody] ExecuteTransferDto dto)
    {
        var transferId = await _transfers.ExecuteTransferAsync(dto, _currentUser.Email!);
        return Ok(new { transferId });
    }

    [HttpPost("{transferId:guid}/reverse")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ReverseTransfer(Guid transferId, [FromBody] ReverseTransferDto dto)
    {
        await _transfers.ReverseTransferAsync(transferId, dto.Reason, _currentUser.Email!);
        return NoContent();
    }
}
