using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Update.v1;

public sealed record UpdateICSLineItemRequest(
    string PropertyCode,
    string Description,
    int Quantity,
    DateTime DateAcquired,
    decimal UnitCost,
    string? Condition = null,
    string? Remarks = null);

public sealed record UpdateICSCommand(
    Guid Id,
    Guid EmployeeId,
    DateTime IssuanceDate,
    IReadOnlyList<UpdateICSLineItemRequest> LineItems,
    string? IssuancePurpose = null,
    string? IssuanceLocation = null,
    string? Notes = null) : IRequest<UpdateICSResponse>;

public sealed record UpdateICSResponse(
    Guid Id,
    string ICSNumber,
    DateTime UpdatedAt);
