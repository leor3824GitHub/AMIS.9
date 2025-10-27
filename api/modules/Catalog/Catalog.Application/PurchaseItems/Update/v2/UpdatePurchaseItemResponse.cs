namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Update.v2;
public sealed record UpdatePurchaseItemResponse(Guid? Id,
    bool Success = true, // Default to true if not specified (indicating success)
    string? ErrorMessage = null); // Optional error message
