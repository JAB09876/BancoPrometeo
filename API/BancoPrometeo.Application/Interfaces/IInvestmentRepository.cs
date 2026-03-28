using BancoPrometeo.Application.Features.Investments.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface IInvestmentRepository
{
    Task<Guid> CreateInvestmentAsync(CreateInvestmentDto dto, string createdBy);
    Task<IEnumerable<InvestmentSummaryDto>> GetAllInvestmentsAsync();
    Task<IEnumerable<InvestmentSummaryDto>> GetCustomerInvestmentsAsync(Guid customerId);
    Task<InvestmentDetailDto?> GetInvestmentByIdAsync(Guid investmentId);
    Task<decimal> SimulateReturnAsync(decimal principal, decimal annualRate, int days);
}
