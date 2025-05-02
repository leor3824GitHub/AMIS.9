using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.Create.v1;

public sealed record CreateInspectionCommand(
    [property: DefaultValue("2024-05-01")] DateTime InspectionDate,
    [property: DefaultValue("bfb91a20-xxxx-xxxx-xxxx-df0c914c1a22")] Guid InspectedBy,
    [property: DefaultValue("beef1122-xxxx-xxxx-xxxx-aabbccddeeff")] Guid PurchaseId,
    [property: DefaultValue("Initial inspection remarks")] string? Remarks,
    List<InspectionItemDto> Items
) : IRequest<CreateInspectionResponse>;

public sealed record InspectionItemDto(
    Guid PurchaseItemId,
    int QuantityInspected,
    string? Remarks
);
