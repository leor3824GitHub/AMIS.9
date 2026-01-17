using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactions.Get.v1;

public sealed record GetInventoryTransactionRequest(Guid Id) : IRequest<InventoryTransactionResponse>;

