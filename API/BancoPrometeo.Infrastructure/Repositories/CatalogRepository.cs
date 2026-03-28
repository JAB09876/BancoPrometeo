using BancoPrometeo.Application.Features.Catalogs.DTOs;
using BancoPrometeo.Application.Interfaces;
using Dapper;

namespace BancoPrometeo.Infrastructure.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CatalogRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<AccountTypeDto>> GetAccountTypesAsync()
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<AccountTypeDto>("SELECT Code, Name, Description, IsActive FROM Cat.AccountTypes WHERE IsActive = 1");
    }

    public async Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync()
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<CurrencyDto>("SELECT Code, Name, Symbol, IsActive FROM Cat.Currencies WHERE IsActive = 1");
    }

    public async Task<IEnumerable<TransactionTypeDto>> GetTransactionTypesAsync()
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<TransactionTypeDto>("SELECT Code, Name, Direction FROM Cat.TransactionTypes");
    }

    public async Task<IEnumerable<ServiceProviderDto>> GetServiceProvidersAsync()
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<ServiceProviderDto>("SELECT ProviderId, Name, Category, IsActive FROM Cat.ServiceProviders WHERE IsActive = 1");
    }

    public async Task<IEnumerable<InvestmentProductDto>> GetInvestmentProductsAsync()
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryAsync<InvestmentProductDto>("SELECT ProductId, Name, MinAmount, MaxAmount, AnnualRate, MinTermDays, MaxTermDays, IsActive FROM Cat.InvestmentProducts WHERE IsActive = 1");
    }
}
