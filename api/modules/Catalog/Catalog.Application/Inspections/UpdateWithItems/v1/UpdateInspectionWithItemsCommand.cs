using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.UpdateWithItems.v1;

public sealed record UpdateInspectionWithItemsCommand(
    Guid Id,
    Guid? InspectorId = null,
    DateTime? InspectionDate = null,
    string? Remarks = null,
    IReadOnlyList<InspectionItemUpsert>? Items = null,
    IReadOnlyList<Guid>? DeletedItemIds = null
) : IRequest<UpdateInspectionWithItemsResponse>;

public sealed record InspectionItemUpsert(
    Guid? Id = null,
    Guid PurchaseItemId = default,
    int QtyInspected = 0,
    int QtyPassed = 0,
    int QtyFailed = 0,
    string? Remarks = null,
    InspectionItemStatus? InspectionItemStatus = null
);

public sealed record UpdateInspectionWithItemsResponse(
    Guid InspectionId,
    IReadOnlyList<Guid> UpdatedItemIds
);