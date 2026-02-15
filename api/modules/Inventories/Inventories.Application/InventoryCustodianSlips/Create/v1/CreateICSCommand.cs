using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Create.v1;

public sealed record CreateICSLineItemRequest(
    string PropertyCode,
    string Description,
    int Quantity,
    DateTime DateAcquired,
    decimal UnitCost,
    string? Condition = null,
    string? Remarks = null);

public sealed record CreateICSCommand(
    string ICSNumber,
    Guid EmployeeId,
    DateTime IssuanceDate,
    IReadOnlyList<CreateICSLineItemRequest> LineItems,
    string? IssuancePurpose = null,
    string? IssuanceLocation = null,
    string? Notes = null) : IRequest<CreateICSResponse>;

public sealed record CreateICSResponse(
    Guid Id,
    string ICSNumber,
    DateTime CreatedAt);
