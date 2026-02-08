using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Update.v1;

public sealed record UpdatePpeIssuanceReportCommand(
    Guid Id,
    string IssuedTo,
    string Address,
    string Type,
    DateTime Date,
    IReadOnlyList<UpdatePpeIssuanceLineItemRequest> LineItems,
    string? Notes = null) : IRequest<UpdatePpeIssuanceReportResponse>;

public sealed record UpdatePpeIssuanceLineItemRequest(
    string PropertyCode);

public sealed record UpdatePpeIssuanceReportResponse(Guid Id);
