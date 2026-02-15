using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Features.Return.v1;

public sealed record ReturnICSCommand(
    Guid Id,
    DateTime ReturnDate,
    Guid ReceivedByEmployeeId,
    string? ReturnRemarks = null) : IRequest<ReturnICSResponse>;

public sealed record ReturnICSResponse(Guid Id, string Message);
