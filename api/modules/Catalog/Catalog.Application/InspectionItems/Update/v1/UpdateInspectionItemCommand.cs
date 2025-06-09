using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Update.v1;

public sealed record UpdateInspectionItemCommand(
    Guid Id,
    Guid InspectionId,
    Guid PurchaseItemId,
    [property: DefaultValue(0)] int QtyInspected,
    [property: DefaultValue(0)] int QtyPassed,
    [property: DefaultValue(0)] int QtyFailed,
    [property: DefaultValue("Inspected")] string? Remarks
) : IRequest<UpdateInspectionItemResponse>;
