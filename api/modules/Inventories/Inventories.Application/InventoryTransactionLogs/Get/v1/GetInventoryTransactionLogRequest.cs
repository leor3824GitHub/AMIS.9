using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Get.v1;

public sealed record GetInventoryTransactionLogRequest(Guid Id) : IRequest<InventoryTransactionLogResponse>;
