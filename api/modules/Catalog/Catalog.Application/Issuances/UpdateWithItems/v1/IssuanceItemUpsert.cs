namespace AMIS.WebApi.Catalog.Application.Issuances.UpdateWithItems.v1;

public sealed record IssuanceItemUpsert(
    Guid? Id,
    Guid ProductId,
    int Qty,
    decimal UnitPrice,
    string? Status
);