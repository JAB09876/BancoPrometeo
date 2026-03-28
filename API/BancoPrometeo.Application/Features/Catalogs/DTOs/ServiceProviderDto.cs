namespace BancoPrometeo.Application.Features.Catalogs.DTOs;

public record ServiceProviderDto(Guid ProviderId, string Name, string Category, bool IsActive);
