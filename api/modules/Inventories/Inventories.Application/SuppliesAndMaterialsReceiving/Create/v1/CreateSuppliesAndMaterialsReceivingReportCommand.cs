using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Create.v1;

public sealed record CreateSuppliesAndMaterialsReceivingReportCommand(
    string SmrrNumber,
    string Location,
    string SourceName,
    string SourceAddress,
    DateTime ReceivingDate,
    string TransactionType,
    IReadOnlyList<CreateReceivingLineItemRequest> LineItems,
    string ReceivedByName,
    string ReceivedBySignature,
    DateTime ReceivedDate,
    string NotedByName,
    string NotedBySignature,
    DateTime NotedDate,
    string? Notes = null) : IRequest<CreateSuppliesAndMaterialsReceivingReportResponse>;

public sealed record CreateReceivingLineItemRequest(
    string Name,
    string Description,
    DateTime AcquisitionDate,
    decimal Quantity,
    string Unit,
    decimal UnitCost,
    string? Reference = null);

public sealed record CreateSuppliesAndMaterialsReceivingReportResponse(Guid? Id);

