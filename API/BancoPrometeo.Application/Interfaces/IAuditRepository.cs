using BancoPrometeo.Application.Features.Audit.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface IAuditRepository
{
    Task<IEnumerable<AuditTrailDto>> GetEntityAuditTrailAsync(string tableName, Guid recordId, DateTime? fromDate, DateTime? toDate);
}
