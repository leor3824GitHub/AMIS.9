namespace AMIS.WebApi.Catalog.Application.Inventories.ReleaseReservation.v1;

public record ReleaseReservationResponse(
    Guid InventoryId,
    int ReleasedQty,
    int RemainingReservedQty,
    int AvailableQty,
    string Message
);
