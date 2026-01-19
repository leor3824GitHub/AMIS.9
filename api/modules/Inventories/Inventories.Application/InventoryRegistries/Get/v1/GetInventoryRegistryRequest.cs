using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryRegistries.Get.v1;

public sealed record GetInventoryRegistryRequest(Guid Id) : IRequest<InventoryRegistryResponse>;
