using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Update.v1;

public sealed record UpdatePpeIssuanceReportCommand(
    Guid Id,
    string RecipientName,
    string RecipientAddress,
    string IssuanceType,
    DateTime IssuanceDate,
    IReadOnlyList<UpdatePpeIssuanceLineItemRequest> LineItems,
    string? Notes = null) : IRequest<UpdatePpeIssuanceReportResponse>;

public sealed record UpdatePpeIssuanceLineItemRequest(
    string PropertyCode,
    string Description,
    decimal Quantity,
    string Unit,
    DateTime? DateAcquired,
    decimal AcquisitionCost,
    decimal? AccumulatedDepreciation,
    decimal? BookValue,
    string? Location);

public sealed record UpdatePpeIssuanceReportResponse(Guid Id);
