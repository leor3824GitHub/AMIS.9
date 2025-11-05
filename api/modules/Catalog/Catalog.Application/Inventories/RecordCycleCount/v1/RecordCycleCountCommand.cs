using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.RecordCycleCount.v1;

public record RecordCycleCountCommand(
    Guid InventoryId,
    int CountedQty,
    DateTime CountDate) : IRequest<RecordCycleCountResponse>;
