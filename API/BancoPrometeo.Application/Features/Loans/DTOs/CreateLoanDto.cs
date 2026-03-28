namespace BancoPrometeo.Application.Features.Loans.DTOs;

public record CreateLoanDto(
    Guid CustomerId,
    decimal Principal,
    decimal AnnualInterestRate,
    int TermMonths,
    string? Purpose
);
