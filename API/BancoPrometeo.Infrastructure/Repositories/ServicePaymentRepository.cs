using System.Data;
using BancoPrometeo.Application.Features.ServicePayments.DTOs;
using BancoPrometeo.Application.Interfaces;
using Dapper;

namespace BancoPrometeo.Infrastructure.Repositories;

public class ServicePaymentRepository : IServicePaymentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ServicePaymentRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> PayServiceAsync(PayServiceDto dto, string createdBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@CustomerId", dto.CustomerId);
        p.Add("@SourceAccountId", dto.SourceAccountId);
        p.Add("@ServiceProviderId", dto.ServiceProviderId);
        p.Add("@ContractNumber", dto.ContractNumber);
        p.Add("@Amount", dto.Amount);
        p.Add("@CreatedBy", createdBy);
        p.Add("@PaymentId", dbType: DbType.Guid, direction: ParameterDirection.Output);

        await conn.ExecuteAsync("Core.sp_PayService", p, commandType: CommandType.StoredProcedure);
        return p.Get<Guid>("@PaymentId");
    }
}
