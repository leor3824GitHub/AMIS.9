using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Create.v1;

public sealed record CreateSuppliesAndMaterialsReceivingReportCommand(
    string SmrrNumber,
    string SourceName,
    string SourceAddress,
    DateTime ReceivingDate,
    string TransactionType,
    IReadOnlyList<CreateReceivingLineItemRequest> LineItems,
    string ReceivedByName,
    DateTime ReceivedDate,
    string NotedByName,
    DateTime NotedDate,
    string? Notes = null) : IRequest<CreateSuppliesAndMaterialsReceivingReportResponse>;

public sealed record CreateReceivingLineItemRequest(
    string Name,
    string Description,
    DateTime AcquisitionDate,
    decimal Quantity,
    string Unit,
    decimal UnitCost,
    string Location,
    string? Reference = null,
    string? ClassCode = null,
    string? CategoryCode = null,
    string? ItemCode = null);

public sealed record CreateSuppliesAndMaterialsReceivingReportResponse(Guid? Id);

