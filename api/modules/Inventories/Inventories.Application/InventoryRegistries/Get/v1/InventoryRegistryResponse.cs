using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.InventoryRegistries.Get.v1;

public sealed record InventoryRegistryResponse(
    Guid Id,
    string PropertyCode,
    string Description,
    int Quantity,
    InventoryItemStatus Status,
    string Location,
    DateTime ReceivedDate,
    DateTime? IssuedDate,
    DateTime LastTransactionDate,
    string LastTransactionType,
    string LastTransactionReference);
