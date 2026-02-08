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
    string EmployeeName,
    string Department,
    DateTime IssuanceDate,
    IReadOnlyList<CreatePARLineItemRequest> LineItems,
    string? Position = null,
    string? IssuancePurpose = null,
    string? IssuanceLocation = null,
    string? Notes = null,
    string? IssuedByName = null,
    string? ReceivedByName = null,
    string? ApprovedByName = null) : IRequest<CreatePARResponse>;

public sealed record CreatePARResponse(
    Guid Id,
    string PARNumber,
    DateTime CreatedAt);
