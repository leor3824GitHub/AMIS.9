using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SemexRegistryDomain = AMIS.WebApi.Inventories.Domain.SemexRegistry;

namespace AMIS.WebApi.Inventories.Application.SemexRegistry.Search.v1;

public sealed class SearchSemexRegistriesHandler(
    ILogger<SearchSemexRegistriesHandler> logger,
    [FromKeyedServices("inventories:semex-registries")] IReadRepository<SemexRegistryDomain> repository)
    : IRequestHandler<SearchSemexRegistriesCommand, PagedList<SemexRegistryResponse>>
{
    public async Task<PagedList<SemexRegistryResponse>> Handle(
        SearchSemexRegistriesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Searching Semex registries with filters - ItemCode: {ItemCode}, Description: {Description}, Location: {Location}, Page: {PageNumber}",
            request.ItemCode, request.Description, request.Location, request.PageNumber);

        var spec = new SearchSemexRegistriesSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        // Get total count without pagination for PagedList
        var countSpec = new SearchSemexRegistriesSpecForCount(request);
        var totalCount = await repository.CountAsync(countSpec, cancellationToken).ConfigureAwait(false);

        var list = items.Select(r => new SemexRegistryResponse(
            r.Id,
            r.ItemCode,
            r.Description,
            r.Quantity,
            r.Unit,
            r.UnitCost,
            r.Location,
            r.Status.ToString(),
            r.ReceivedDate,
            r.IssuedDate,
            r.LastTransactionDate,
            r.LastTransactionType,
            r.LastTransactionReference))
            .ToList();

        logger.LogInformation("Found {Count} Semex registry entries", totalCount);

        return new PagedList<SemexRegistryResponse>(
            list,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}
