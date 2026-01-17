using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.Canvasses.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Canvasses.Search.v1;

public sealed class SearchCanvassesHandler(
    [FromKeyedServices("inventories:canvasses")] IReadRepository<Canvass> repository)
    : IRequestHandler<SearchCanvassesCommand, PagedList<CanvassResponse>>
{
    public async Task<PagedList<CanvassResponse>> Handle(SearchCanvassesCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchCanvassesSpecs(request);

        var canvasses = await repository.ListAsync(spec, cancellationToken);
        var count = await repository.CountAsync(spec, cancellationToken);

        var canvassResponses = canvasses.Select(c => new CanvassResponse(
            c.Id,
            c.PurchaseRequestId,
            c.SupplierId,
            c.ItemDescription,
            c.Quantity,
            c.Unit,
            c.QuotedPrice,
            c.Remarks,
            c.ResponseDate,
            c.IsSelected)).ToList();

        return new PagedList<CanvassResponse>(canvassResponses, count, request.PageNumber, request.PageSize);
    }
}

