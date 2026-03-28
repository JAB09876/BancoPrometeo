namespace BancoPrometeo.Application.Features.Catalogs.DTOs;

public record InvestmentProductDto(Guid ProductId, string Name, decimal MinAmount, decimal MaxAmount, decimal AnnualRate, int MinTermDays, int MaxTermDays, bool IsActive);
