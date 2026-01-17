using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Create.v1;

public sealed record CreatePpeIssuanceLineItemRequest(
    string PropertyCode,
    string Description,
    decimal Quantity,
    string Unit,
    decimal AcquisitionCost,
    decimal? AccumulatedDepreciation = null,
    decimal? BookValue = null);

public sealed record CreatePpeIssuanceReportCommand(
    string ReportNumber,
    string RecipientName,
    string RecipientAddress,
    string IssuanceType,
    DateTime IssuanceDate,
    IReadOnlyList<CreatePpeIssuanceLineItemRequest> LineItems,
    string? Notes = null) : IRequest<CreatePpeIssuanceReportResponse>;

public sealed record CreatePpeIssuanceReportResponse(
    Guid Id,
    string ReportNumber,
    DateTime CreatedAt);

