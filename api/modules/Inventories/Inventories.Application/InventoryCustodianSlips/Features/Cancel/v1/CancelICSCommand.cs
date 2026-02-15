using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Features.Cancel.v1;

public sealed record CancelICSCommand(Guid Id) : IRequest<CancelICSResponse>;

public sealed record CancelICSResponse(Guid Id, string Message);
