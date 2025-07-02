using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using System.ComponentModel;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Create.v1;

public sealed record CreateInspectionItemCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000001")] Guid InspectionId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000002")] Guid PurchaseItemId,
    [property: DefaultValue(10)] int QtyInspected,
    [property: DefaultValue(9)] int QtyPassed,
    [property: DefaultValue(1)] int QtyFailed,
    [property: DefaultValue("Minor defects noted")] string? Remarks,
    [property: DefaultValue(InspectionItemStatus.Passed)] InspectionItemStatus? InspectionItemStatus
) : IRequest<CreateInspectionItemResponse>;
