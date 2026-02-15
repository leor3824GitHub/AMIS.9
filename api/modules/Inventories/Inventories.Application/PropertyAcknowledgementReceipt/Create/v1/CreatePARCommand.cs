using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Create.v1;

public sealed record CreatePARLineItemRequest(
    string PropertyCode,
    string Description,
    DateTime DateAcquired,
    decimal AcquisitionCost,
    string? Condition = null,
    string? Remarks = null);

public sealed record CreatePARCommand(
    string PARNumber,
    Guid EmployeeId,
    DateTime IssuanceDate,
    IReadOnlyList<CreatePARLineItemRequest> LineItems,
    string? IssuancePurpose = null,
    string? IssuanceLocation = null,
    string? Notes = null) : IRequest<CreatePARResponse>;

public sealed record CreatePARResponse(
    Guid Id,
    string PARNumber,
    DateTime CreatedAt);
