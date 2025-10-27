using System.ComponentModel;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Update.v1;

public sealed record UpdateInspectionItemCommand(
    Guid Id,
    Guid InspectionId,
    Guid PurchaseItemId,
    [property: DefaultValue(0)] int QtyInspected,
    [property: DefaultValue(0)] int QtyPassed,
    [property: DefaultValue(0)] int QtyFailed,
    [property: DefaultValue("Inspected")] string? Remarks,
    [property: DefaultValue(InspectionItemStatus.Passed)] InspectionItemStatus? InspectionItemStatus
) : IRequest<UpdateInspectionItemResponse>;
