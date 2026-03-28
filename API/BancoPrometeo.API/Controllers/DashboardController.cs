using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IReportRepository _reports;

    public DashboardController(IReportRepository reports)
    {
        _reports = reports;
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAdminDashboard()
    {
        var dashboard = await _reports.GetAdminDashboardAsync();
        return Ok(dashboard);
    }
}
