using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Domain;
using Ardalis.Specification;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Search.v1;

public sealed class SearchInventoryTransactionLogsSpecs : EntitiesByPaginationFilterSpec<InventoryTransactionLog>
{
    public SearchInventoryTransactionLogsSpecs(SearchInventoryTransactionLogsCommand command)
        : base(command)
    {
        Query
            .Where(x => x.PropertyCode == command.PropertyCode, !string.IsNullOrWhiteSpace(command.PropertyCode))
            .Where(x => x.TransactionType == command.TransactionType, !string.IsNullOrWhiteSpace(command.TransactionType))
            .Where(x => x.Success == command.Success, command.Success.HasValue)
            .Where(x => x.TransactionDate >= command.FromDate!.Value, command.FromDate.HasValue)
            .Where(x => x.TransactionDate <= command.ToDate!.Value, command.ToDate.HasValue)
            .OrderByDescending(x => x.TransactionDate);
    }
}
