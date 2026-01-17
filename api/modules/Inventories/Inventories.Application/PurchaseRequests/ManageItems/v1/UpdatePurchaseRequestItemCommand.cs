using MediatR;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.ManageItems.v1;

public sealed record UpdatePurchaseRequestItemCommand(
    Guid PurchaseRequestId,
    Guid ItemId,
    Guid? ProductId,
    string? ManualProductName,
    int Qty,
    string Unit,
    string? Description
) : IRequest;

