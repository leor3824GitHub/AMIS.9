using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Update.v1;

public sealed record UpdatePARLineItemRequest(
    string PropertyCode,
    string Description,
    DateTime DateAcquired,
    decimal AcquisitionCost,
    string? Condition = null,
    string? Remarks = null);

public sealed record UpdatePARCommand(
    Guid Id,
    Guid EmployeeId,
    DateTime IssuanceDate,
    IReadOnlyList<UpdatePARLineItemRequest> LineItems,
    string? IssuancePurpose = null,
    string? IssuanceLocation = null,
    string? Notes = null) : IRequest<UpdatePARResponse>;

public sealed record UpdatePARResponse(
    Guid Id,
    string PARNumber,
    DateTime UpdatedAt);
