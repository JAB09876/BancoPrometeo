using BancoPrometeo.Application.Features.Transfers.DTOs;

namespace BancoPrometeo.Application.Interfaces;

public interface ITransferRepository
{
    Task<Guid> ExecuteTransferAsync(ExecuteTransferDto dto, string createdBy);
    Task ReverseTransferAsync(Guid transferId, string reason, string reversedBy);
}
