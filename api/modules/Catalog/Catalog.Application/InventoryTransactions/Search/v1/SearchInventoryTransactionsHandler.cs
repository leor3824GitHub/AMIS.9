using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.InventoryTransactions.Get.v1;
using AMIS.WebApi.Catalog.Application.Products.Search.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Search.v1;

public sealed class SearchInventoryTransactionsHandler(
    [FromKeyedServices("catalog:inventory-transactions")] IReadRepository<InventoryTransaction> repository)
    : IRequestHandler<SearchInventoryTransactionsCommand, PagedList<InventoryTransactionResponse>>
{
    public async Task<PagedList<InventoryTransactionResponse>> Handle(SearchInventoryTransactionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInventoryTransactionSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InventoryTransactionResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
