namespace AMIS.WebApi.Inventories.Application.IssuanceItems.Create.v1;
public sealed record CreateIssuanceItemResponse(Guid? Id,
    bool Success = true, // Default to true if not specified (indicating success)
    string? ErrorMessage = null); // Optional error message

