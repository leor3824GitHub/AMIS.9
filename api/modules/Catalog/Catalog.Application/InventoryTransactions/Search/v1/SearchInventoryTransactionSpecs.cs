using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.InventoryTransactions.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using Ardalis.Specification;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Search.v1;

public sealed class SearchInventoryTransactionSpecs : EntitiesByPaginationFilterSpec<InventoryTransaction, InventoryTransactionResponse>
{
    public SearchInventoryTransactionSpecs(SearchInventoryTransactionsCommand command)
        : base(command)
    {
        Query
            .OrderByDescending(t => t.Created, !command.HasOrderBy())
            .Where(t => t.ProductId == command.ProductId!.Value, command.ProductId.HasValue)
            .Where(t => t.SourceId == command.SourceId!.Value, command.SourceId.HasValue);

        if (!string.IsNullOrWhiteSpace(command.TransactionType)
            && Enum.TryParse<TransactionType>(command.TransactionType, true, out var parsedType))
        {
            Query.Where(t => t.TransactionType == parsedType);
        }
    }
}
