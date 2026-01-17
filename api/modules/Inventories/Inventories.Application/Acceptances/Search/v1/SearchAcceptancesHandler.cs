using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.Acceptances.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Search.v1;

public sealed class SearchAcceptancesHandler(
    [FromKeyedServices("inventories:acceptances")] IReadRepository<Acceptance> repository)
    : IRequestHandler<SearchAcceptancesCommand, PagedList<AcceptanceResponse>>
{
    public async Task<PagedList<AcceptanceResponse>> Handle(SearchAcceptancesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAcceptanceSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AcceptanceResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

