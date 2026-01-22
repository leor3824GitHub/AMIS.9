using AMIS.WebApi.Inventories.Application.SemexRegistry.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AMIS.Framework.Core.Persistence;
using SemexRegistryDomain = AMIS.WebApi.Inventories.Domain.SemexRegistry;

namespace AMIS.WebApi.Inventories.Application.SemexRegistry.Get.v1;

public sealed record GetSemexRegistryByIdQuery(Guid Id) : IRequest<SemexRegistryDetailResponse>;

public sealed record SemexRegistryDetailResponse(
    Guid Id,
    string ItemCode,
    string Description,
    int Quantity,
    string Unit,
    decimal UnitCost,
    string Location,
    string Status,
    DateTime ReceivedDate,
    DateTime? IssuedDate,
    DateTime LastTransactionDate,
    string LastTransactionType,
    string LastTransactionReference);

public sealed class GetSemexRegistryByIdHandler(
    ILogger<GetSemexRegistryByIdHandler> logger,
    [FromKeyedServices("inventories:semex-registries")] IReadRepository<SemexRegistryDomain> repository)
    : IRequestHandler<GetSemexRegistryByIdQuery, SemexRegistryDetailResponse>
{
    public async Task<SemexRegistryDetailResponse> Handle(
        GetSemexRegistryByIdQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var registry = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (registry is null)
        {
            logger.LogWarning("Semex registry not found with ID {RegistryId}", request.Id);
            throw new InvalidOperationException($"Semex registry with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved Semex registry {ItemCode}", registry.ItemCode);

        return new SemexRegistryDetailResponse(
            registry.Id,
            registry.ItemCode,
            registry.Description,
            registry.Quantity,
            registry.Unit,
            registry.UnitCost,
            registry.Location,
            registry.Status.ToString(),
            registry.ReceivedDate,
            registry.IssuedDate,
            registry.LastTransactionDate,
            registry.LastTransactionType,
            registry.LastTransactionReference);
    }
}
