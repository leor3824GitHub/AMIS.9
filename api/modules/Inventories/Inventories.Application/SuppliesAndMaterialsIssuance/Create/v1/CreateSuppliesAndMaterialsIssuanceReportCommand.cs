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
    string IssuingOfficerSignature,
    DateTime IssuingDate,
    string ApprovingOfficerName,
    string ApprovingOfficerSignature,
    DateTime ApprovingDate,
    string AuthRecipientName,
    string AuthRecipientSignature,
    DateTime AuthReceiptDate,
    string? DriverName = null,
    string? DriverSignature = null,
    string? BillOfLadingNumber = null,
    string? Notes = null) : IRequest<CreateSuppliesAndMaterialsIssuanceReportResponse>;

public sealed record CreateIssuanceLineItemRequest(
    string Name,
    string Description,
    DateTime AcquisitionDate,
    decimal Quantity,
    string Unit,
    decimal UnitCost);

public sealed record CreateSuppliesAndMaterialsIssuanceReportResponse(Guid? Id);

