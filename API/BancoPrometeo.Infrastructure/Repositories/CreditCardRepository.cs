using System.Data;
using BancoPrometeo.Application.Features.CreditCards.DTOs;
using BancoPrometeo.Application.Interfaces;
using Dapper;

namespace BancoPrometeo.Infrastructure.Repositories;

public class CreditCardRepository : ICreditCardRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CreditCardRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task PayCreditCardAsync(Guid cardId, PayCreditCardDto dto, string paidBy)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new DynamicParameters();
        p.Add("@CardId", cardId);
        p.Add("@SourceAccountId", dto.SourceAccountId);
        p.Add("@Amount", dto.Amount);
        p.Add("@PaidBy", paidBy);

        await conn.ExecuteAsync("Core.sp_PayCreditCard", p, commandType: CommandType.StoredProcedure);
    }
}
