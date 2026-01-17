using MediatR;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.ManageItems.v1;

public sealed record AddPurchaseRequestItemCommand(
    Guid PurchaseRequestId,
    Guid? ProductId,
    string? ManualProductName,
    int Qty,
    string Unit,
    string? Description
) : IRequest<Guid>; // returns new item Id

