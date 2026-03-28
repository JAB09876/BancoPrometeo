using BancoPrometeo.Application.Features.Audit.DTOs;
using BancoPrometeo.Application.Interfaces;
using Dapper;

namespace BancoPrometeo.Infrastructure.Repositories;

public class AuditRepository : IAuditRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AuditRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<AuditTrailDto>> GetEntityAuditTrailAsync(string tableName, Guid recordId, DateTime? fromDate, DateTime? toDate)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@TableName", tableName);
        p.Add("@RecordId", recordId);
        p.Add("@FromDate", fromDate);
        p.Add("@ToDate", toDate);

        return await conn.QueryAsync<AuditTrailDto>(
            "Audit.sp_GetEntityAuditTrail", p, commandType: System.Data.CommandType.StoredProcedure);
    }
}
