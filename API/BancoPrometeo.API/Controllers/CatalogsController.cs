using BancoPrometeo.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoPrometeo.API.Controllers;

[ApiController]
[Route("api/catalogs")]
public class CatalogsController : ControllerBase
{
    private readonly ICatalogRepository _catalogs;

    public CatalogsController(ICatalogRepository catalogs)
    {
        _catalogs = catalogs;
    }

    [HttpGet("account-types")]
    public async Task<IActionResult> GetAccountTypes()
        => Ok(await _catalogs.GetAccountTypesAsync());

    [HttpGet("currencies")]
    public async Task<IActionResult> GetCurrencies()
        => Ok(await _catalogs.GetCurrenciesAsync());

    [HttpGet("transaction-types")]
    public async Task<IActionResult> GetTransactionTypes()
        => Ok(await _catalogs.GetTransactionTypesAsync());

    [HttpGet("service-providers")]
    public async Task<IActionResult> GetServiceProviders()
        => Ok(await _catalogs.GetServiceProvidersAsync());

    [HttpGet("investment-products")]
    public async Task<IActionResult> GetInvestmentProducts()
        => Ok(await _catalogs.GetInvestmentProductsAsync());
}
