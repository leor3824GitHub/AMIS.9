using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.UpdateItem.v1;

public sealed record UpdateInspectionItemCommand(
    Guid Id,
    Guid InspectionId,
    Guid PurchaseItemId,
    int QuantityInspected,
    int QuantityPassed,
    int QuantityFailed,
    string? Remarks
) : IRequest<Guid>;
