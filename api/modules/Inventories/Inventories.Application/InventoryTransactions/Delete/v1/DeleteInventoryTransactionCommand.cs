using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactions.Delete.v1;

public sealed record DeleteInventoryTransactionCommand(Guid Id) : IRequest<DeleteInventoryTransactionResponse>;

