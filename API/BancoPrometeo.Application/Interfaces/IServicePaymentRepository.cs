using BancoPrometeo.Application.Features.ServicePayments.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface IServicePaymentRepository
{
    Task<Guid> PayServiceAsync(PayServiceDto dto, string createdBy);
}
