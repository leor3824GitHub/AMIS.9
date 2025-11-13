using MediatR;

namespace AMIS.WebApi.Catalog.Application.Acceptances.ManageItems.v1;

public sealed record AddAcceptanceItemCommand(
    Guid AcceptanceId,
    Guid PurchaseItemId,
    int QtyAccepted,
    string? Remarks
) : IRequest<AddAcceptanceItemResponse>;
