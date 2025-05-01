using MediatR;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Get.v1;

public sealed record GetInventoryTransactionRequest(Guid Id) : IRequest<InventoryTransactionResponse>;
