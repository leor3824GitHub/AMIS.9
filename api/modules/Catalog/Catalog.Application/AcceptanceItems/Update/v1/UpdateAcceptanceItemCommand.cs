using MediatR;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Update.v1;

public sealed record UpdateAcceptanceItemCommand(
    Guid Id,
    Guid AcceptanceId,
    Guid PurchaseItemId,
    int QtyAccepted,
    string? Remarks
) : IRequest<UpdateAcceptanceItemResponse>;
