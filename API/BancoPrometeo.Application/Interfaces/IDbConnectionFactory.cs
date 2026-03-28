using System.Data;

namespace BancoPrometeo.Application.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
