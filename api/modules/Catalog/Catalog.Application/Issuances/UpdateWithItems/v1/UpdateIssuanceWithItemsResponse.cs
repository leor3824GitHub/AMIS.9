namespace AMIS.WebApi.Catalog.Application.Issuances.UpdateWithItems.v1;

public sealed record UpdateIssuanceWithItemsResponse(
    Guid IssuanceId,
    IReadOnlyList<Guid> ItemIds
);