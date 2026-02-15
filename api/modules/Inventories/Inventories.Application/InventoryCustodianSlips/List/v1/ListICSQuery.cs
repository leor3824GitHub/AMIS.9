using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.List.v1;

public sealed record ListICSQuery(int PageNumber = 1, int PageSize = 10) : IRequest<ListICSResponse>;

public sealed record ListICSItemResponse(
    Guid Id,
    string ICSNumber,
    Guid EmployeeId,
    DateTime IssuanceDate,
    string Status,
    int LineItemCount,
    decimal TotalAcquisitionCost);

public sealed record ListICSResponse(
    IReadOnlyList<ListICSItemResponse> Items,
    int Total,
    int PageNumber,
    int PageSize);
