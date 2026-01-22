using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Get.v1;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryRegistries.Search.v1;

public sealed class SearchInventoryRegistriesCommand : PaginationFilter, IRequest<PagedList<InventoryRegistryResponse>>
{
    public string? PropertyCode { get; set; }
    public string? Description { get; set; }
    public InventoryItemStatus? Status { get; set; }
    public string? Location { get; set; }
}
