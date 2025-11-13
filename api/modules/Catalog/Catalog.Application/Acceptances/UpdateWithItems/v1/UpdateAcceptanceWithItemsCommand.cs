using MediatR;

namespace AMIS.WebApi.Catalog.Application.Acceptances.UpdateWithItems.v1;

public sealed record UpdateAcceptanceWithItemsCommand(
    Guid Id,
    Guid? SupplyOfficerId = null,
    DateTime? AcceptanceDate = null,
    string? Remarks = null,
    IReadOnlyList<AcceptanceItemUpsert>? Items = null,
    IReadOnlyList<Guid>? DeletedItemIds = null
) : IRequest<UpdateAcceptanceWithItemsResponse>;

public sealed record AcceptanceItemUpsert(
    Guid? Id = null,
    Guid PurchaseItemId = default,
    int QtyAccepted = 0,
    string? Remarks = null
);

public sealed record UpdateAcceptanceWithItemsResponse(
    Guid AcceptanceId,
    IReadOnlyList<Guid> UpdatedItemIds
);