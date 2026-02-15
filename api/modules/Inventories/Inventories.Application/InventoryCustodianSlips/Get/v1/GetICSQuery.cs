using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Get.v1;

public sealed record GetICSQuery(Guid Id) : IRequest<GetICSResponse>;

public sealed record ICSLineItemResponse(
    string PropertyCode,
    string Description,
    int Quantity,
    DateTime DateAcquired,
    decimal UnitCost,
    decimal AcquisitionCost,
    string? Condition,
    string? Remarks);

public sealed record GetICSResponse(
    Guid Id,
    string ICSNumber,
    Guid EmployeeId,
    DateTime IssuanceDate,
    string? IssuancePurpose,
    string? IssuanceLocation,
    string? Notes,
    string Status,
    IReadOnlyList<ICSLineItemResponse> LineItems,
    decimal TotalAcquisitionCost);
