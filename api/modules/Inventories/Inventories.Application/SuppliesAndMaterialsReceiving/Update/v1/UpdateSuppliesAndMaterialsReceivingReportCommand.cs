using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Update.v1;

public sealed record UpdateSuppliesAndMaterialsReceivingReportCommand(
    Guid Id,
    string SourceName,
    string SourceAddress,
    DateTime SourceReceiptDate,
    string ReceiptType,
    IReadOnlyList<UpdateReceivingLineItemRequest> LineItems,
    string? Notes = null) : IRequest<UpdateSuppliesAndMaterialsReceivingReportResponse>;

public sealed record UpdateReceivingLineItemRequest(
    string PropertyCode,
    string Name,
    string Description,
    DateTime AcquisitionDate,
    decimal Quantity,
    string Unit,
    decimal UnitCost,
    string? Location = null,
    string? ClassCode = null,
    string? CategoryCode = null,
    string? ItemCode = null);

public sealed record UpdateSuppliesAndMaterialsReceivingReportResponse(Guid? Id);
