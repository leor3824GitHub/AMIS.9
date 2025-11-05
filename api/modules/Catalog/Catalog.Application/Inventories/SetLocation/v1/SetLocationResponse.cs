namespace AMIS.WebApi.Catalog.Application.Inventories.SetLocation.v1;

public sealed record SetLocationResponse(
    Guid InventoryId,
    string Location,
    string Message
);
