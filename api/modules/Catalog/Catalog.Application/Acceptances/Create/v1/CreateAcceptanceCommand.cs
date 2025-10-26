using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Create.v1;

public sealed record CreateAcceptanceCommand(
    [property: DefaultValue("2024-05-01")] DateTime AcceptanceDate,
    [property: DefaultValue("bfb91a20-xxxx-xxxx-xxxx-df0c914c1a22")] Guid SupplyOfficerId,
    [property: DefaultValue("beef1122-xxxx-xxxx-xxxx-aabbccddeeff")] Guid PurchaseId,
    [property: DefaultValue("Initial inspection remarks")] string? Remarks,
    Guid? InspectionId = null,
    bool PostToInventory = true,
    IReadOnlyCollection<AcceptanceItemDto>? Items = null
) : IRequest<CreateAcceptanceResponse>;

public sealed record AcceptanceItemDto(
    Guid? AcceptanceId,
    Guid PurchaseItemId,
    int QtyAccepted,
    string? Remarks
);
