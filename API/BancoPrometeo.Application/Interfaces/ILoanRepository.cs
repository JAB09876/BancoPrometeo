using BancoPrometeo.Application.Features.Loans.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface ILoanRepository
{
    Task<Guid> CreateLoanRequestAsync(CreateLoanDto dto, string createdBy);
    Task ApproveLoanAsync(Guid loanId, string? notes, string approvedBy);
    Task PayInstallmentAsync(Guid loanId, PayLoanInstallmentDto dto, string paidBy);
    Task<IEnumerable<LoanSummaryDto>> GetAllLoansAsync(string? status);
    Task<IEnumerable<LoanSummaryDto>> GetCustomerLoansAsync(Guid customerId);
    Task<LoanDetailDto?> GetLoanByIdAsync(Guid loanId);
    Task<IEnumerable<LoanInstallmentDto>> GetInstallmentsAsync(Guid loanId);
    Task<IEnumerable<AmortizationRowDto>> GetAmortizationTableAsync(Guid loanId);
    Task<decimal> SimulateMonthlyPaymentAsync(decimal principal, decimal annualRate, int termMonths);
}
