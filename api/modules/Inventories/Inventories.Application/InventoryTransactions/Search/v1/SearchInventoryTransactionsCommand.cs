using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.InventoryTransactions.Get.v1;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactions.Search.v1;

public sealed class SearchInventoryTransactionsCommand : PaginationFilter, IRequest<PagedList<InventoryTransactionResponse>>
{
    public Guid? ProductId { get; set; }
    public Guid? SourceId { get; set; }
    public string? TransactionType { get; set; }
}

