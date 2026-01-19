using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Get.v1;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Search.v1;

public sealed class SearchInventoryTransactionLogsCommand : PaginationFilter, IRequest<PagedList<InventoryTransactionLogResponse>>
{
    public string? PropertyCode { get; set; }
    public string? TransactionType { get; set; }
    public bool? Success { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
