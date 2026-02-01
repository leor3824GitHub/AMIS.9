using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Create.v1;

public sealed record CreateSuppliesAndMaterialsIssuanceReportCommand(
    string SmirNumber,
    DateTime TransactionDate,
    string RecipientName,
    string RecipientAddress,
    string? RecipientContactNumber,
    string IssuanceReason,
    IReadOnlyList<CreateIssuanceLineItemRequest> LineItems,
    string IssuingOfficerName,
    DateTime IssuingDate,
    string ApprovingOfficerName,
    DateTime ApprovingDate,
    string AuthRecipientName,
    DateTime AuthReceiptDate,
    string? DriverName = null,
    string? BillOfLadingNumber = null,
    string? Notes = null) : IRequest<CreateSuppliesAndMaterialsIssuanceReportResponse>;

public sealed record CreateIssuanceLineItemRequest(
    string PropertyCode,
    string Name,
    string Description,
    DateTime AcquisitionDate,
    decimal Quantity,
    string Unit,
    decimal UnitCost);

public sealed record CreateSuppliesAndMaterialsIssuanceReportResponse(Guid? Id);

