using System.Data;
using BancoPrometeo.Application.Features.Transfers.DTOs;
using BancoPrometeo.Application.Interfaces;
using Dapper;

namespace BancoPrometeo.Infrastructure.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TransferRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> ExecuteTransferAsync(ExecuteTransferDto dto, string createdBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@SourceAccountId", dto.SourceAccountId);
        p.Add("@TargetAccountId", dto.TargetAccountId);
        p.Add("@TargetAccountNumber", dto.TargetAccountNumber);
        p.Add("@TargetBankCode", dto.TargetBankCode);
        p.Add("@TargetHolderName", dto.TargetHolderName);
        p.Add("@Amount", dto.Amount);
        p.Add("@CurrencyCode", dto.CurrencyCode);
        p.Add("@Description", dto.Description);
        p.Add("@Channel", dto.Channel);
        p.Add("@CreatedBy", createdBy);
        p.Add("@TransferId", dbType: DbType.Guid, direction: ParameterDirection.Output);

        await conn.ExecuteAsync("Core.sp_ExecuteTransfer", p, commandType: CommandType.StoredProcedure);
        return p.Get<Guid>("@TransferId");
    }

    public async Task ReverseTransferAsync(Guid transferId, string reason, string reversedBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@TransferId", transferId);
        p.Add("@Reason", reason);
        p.Add("@ReversedBy", reversedBy);

        await conn.ExecuteAsync("Core.sp_ReverseTransfer", p, commandType: CommandType.StoredProcedure);
    }
}
