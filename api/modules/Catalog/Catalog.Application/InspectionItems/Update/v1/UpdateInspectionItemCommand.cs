using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Update.v1;

public sealed record UpdateInspectionItemCommand(
    Guid Id,
    Guid InspectionId,
    Guid PurchaseItemId,
    int QtyInspected,
    int QtyPassed,
    int QtyFailed,
    string? Remarks
) : IRequest<Guid>;
