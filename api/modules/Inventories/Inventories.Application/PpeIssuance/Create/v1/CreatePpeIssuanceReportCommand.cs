using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Create.v1;

public sealed record CreatePpeIssuanceLineItemRequest(
    string PropertyCode);

public sealed record CreatePpeIssuanceReportCommand(
    string IRNumber,
    string IssuedTo,
    string Address,
    string Type,
    DateTime Date,
    IReadOnlyList<CreatePpeIssuanceLineItemRequest> LineItems,
    string? Notes = null) : IRequest<CreatePpeIssuanceReportResponse>;

public sealed record CreatePpeIssuanceReportResponse(
    Guid Id,
    string IRNumber);

