using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/audit")]
[Authorize(Roles = "Admin,Auditoria")]
public class AuditController : ControllerBase
{
    private readonly IAuditRepository _audit;

    public AuditController(IAuditRepository audit)
    {
        _audit = audit;
    }

    [HttpGet("{tableName}/{recordId:guid}")]
    public async Task<IActionResult> GetAuditTrail(
        string tableName,
        Guid recordId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var trail = await _audit.GetEntityAuditTrailAsync(tableName, recordId, fromDate, toDate);
        return Ok(trail);
    }
}
