namespace BancoPrometeo.Application.Features.ServicePayments.DTOs;

public record PayServiceDto(
    Guid CustomerId,
    Guid SourceAccountId,
    Guid ServiceProviderId,
    string ContractNumber,
    decimal Amount
);
