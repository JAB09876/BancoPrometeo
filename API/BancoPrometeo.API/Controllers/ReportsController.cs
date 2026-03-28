using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportRepository _reports;
    private readonly ICurrentUserService _currentUser;

    public ReportsController(IReportRepository reports, ICurrentUserService currentUser)
    {
        _reports = reports;
        _currentUser = currentUser;
    }

    [HttpPost("daily-closing")]
    [Authorize(Roles = "Admin,Cajero")]
    public async Task<IActionResult> DailyClosing([FromBody] DateOnly? closingDate = null)
    {
        var result = await _reports.ExecuteDailyClosingAsync(closingDate, _currentUser.Email!);
        return Ok(result);
    }
}
