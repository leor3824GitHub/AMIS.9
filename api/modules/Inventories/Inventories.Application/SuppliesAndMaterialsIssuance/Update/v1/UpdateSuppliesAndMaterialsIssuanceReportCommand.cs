using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Update.v1;

public sealed record UpdateSuppliesAndMaterialsIssuanceReportCommand(
    Guid Id,
    string RecipientName,
    string RecipientAddress,
    string? RecipientContactNumber,
    string IssuanceReason,
    IReadOnlyList<UpdateIssuanceLineItemRequest> LineItems,
    string IssuingOfficerName,
    DateTime IssuingDate,
    string ApprovingOfficerName,
    DateTime ApprovingDate,
    string AuthRecipientName,
    DateTime AuthReceiptDate,
    string? DriverName = null,
    string? BillOfLadingNumber = null,
    string? Notes = null) : IRequest<UpdateSuppliesAndMaterialsIssuanceReportResponse>;

public sealed record UpdateIssuanceLineItemRequest(
    string PropertyCode,
    string Name,
    string Description,
    DateTime AcquisitionDate,
    decimal Quantity,
    string Unit,
    decimal UnitCost);

public sealed record UpdateSuppliesAndMaterialsIssuanceReportResponse(Guid? Id);
