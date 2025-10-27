using MediatR;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Delete.v1;

public sealed record DeleteInventoryTransactionCommand(Guid Id) : IRequest<DeleteInventoryTransactionResponse>;
