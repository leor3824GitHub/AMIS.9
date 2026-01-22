namespace AMIS.WebApi.Inventories.Application.SemexRegistry.Search.v1;

public sealed record SemexRegistryResponse(
    Guid Id,
    string ItemCode,
    string Description,
    int Quantity,
    string Unit,
    decimal UnitCost,
    string Location,
    string Status,
    DateTime ReceivedDate,
    DateTime? IssuedDate,
    DateTime LastTransactionDate,
    string LastTransactionType,
    string LastTransactionReference);
