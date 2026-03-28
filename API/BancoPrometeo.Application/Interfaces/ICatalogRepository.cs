using BancoPrometeo.Application.Features.Catalogs.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface ICatalogRepository
{
    Task<IEnumerable<AccountTypeDto>> GetAccountTypesAsync();
    Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync();
    Task<IEnumerable<TransactionTypeDto>> GetTransactionTypesAsync();
    Task<IEnumerable<ServiceProviderDto>> GetServiceProvidersAsync();
    Task<IEnumerable<InvestmentProductDto>> GetInvestmentProductsAsync();
}
