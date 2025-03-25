using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace AMIS.WebApi.Catalog.Application.Issuances.Search.v1;
public sealed class SearchIssuancesHandler(
    [FromKeyedServices("catalog:issuances")] IReadRepository<Issuance> repository)
    : IRequestHandler<SearchIssuancesCommand, PagedList<IssuanceResponse>>
{
    public async Task<PagedList<IssuanceResponse>> Handle(SearchIssuancesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchIssuanceSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<IssuanceResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}

