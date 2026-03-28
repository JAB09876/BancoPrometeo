namespace BancoPrometeo.Application.Features.Loans.DTOs;

public record PayLoanInstallmentDto(Guid SourceAccountId, decimal Amount);
