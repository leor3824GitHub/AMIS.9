using System.Linq;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Search.v1;

public sealed class SearchInventoryTransactionLogsHandler(
    [FromKeyedServices("inventories:inventory-transaction-logs")] IReadRepository<InventoryTransactionLog> repository)
    : IRequestHandler<SearchInventoryTransactionLogsCommand, PagedList<InventoryTransactionLogResponse>>
{
    public async Task<PagedList<InventoryTransactionLogResponse>> Handle(SearchInventoryTransactionLogsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInventoryTransactionLogsSpecs(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = items.Select(x => new InventoryTransactionLogResponse(
            x.Id,
            x.PropertyCode,
            x.TransactionType,
            x.ReportNumber,
            x.QuantityChange,
            x.InventoryBefore,
            x.InventoryAfter,
            x.StatusBefore,
            x.StatusAfter,
            x.Success,
            x.ErrorMessage,
            x.InitiatedBy,
            x.TransactionDate)).ToList();

        return new PagedList<InventoryTransactionLogResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}
