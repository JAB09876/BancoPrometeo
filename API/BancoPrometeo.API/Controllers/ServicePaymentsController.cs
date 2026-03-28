using BancoPrometeo.Application.Features.ServicePayments.DTOs;
using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/service-payments")]
[Authorize]
public class ServicePaymentsController : ControllerBase
{
    private readonly IServicePaymentRepository _servicePayments;
    private readonly ICurrentUserService _currentUser;

    public ServicePaymentsController(IServicePaymentRepository servicePayments, ICurrentUserService currentUser)
    {
        _servicePayments = servicePayments;
        _currentUser = currentUser;
    }

    [HttpPost]
    [Authorize(Roles = "Cliente,Admin,Cajero")]
    public async Task<IActionResult> PayService([FromBody] PayServiceDto dto)
    {
        var paymentId = await _servicePayments.PayServiceAsync(dto, _currentUser.Email!);
        return Ok(new { paymentId });
    }
}
