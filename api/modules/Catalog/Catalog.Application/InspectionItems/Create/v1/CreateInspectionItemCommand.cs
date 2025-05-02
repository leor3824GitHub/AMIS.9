using MediatR;
using System.ComponentModel;

namespace AMIS.WebApi.Catalog.Application.Inspections.CreateItem.v1;

public sealed record CreateInspectionItemCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000001")] Guid InspectionId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000002")] Guid PurchaseItemId,
    [property: DefaultValue(10)] int QuantityInspected,
    [property: DefaultValue(9)] int QuantityPassed,
    [property: DefaultValue(1)] int QuantityFailed,
    [property: DefaultValue("Minor defects noted")] string? Remarks
) : IRequest<CreateInspectionItemResponse>;
