namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Update.v1;
public sealed record UpdateIssuanceItemResponse(Guid? Id,
    bool Success = true, // Default to true if not specified (indicating success)
    string? ErrorMessage = null); // Optional error message
