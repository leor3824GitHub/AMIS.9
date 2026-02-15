using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Delete.v1;

public sealed record DeleteICSCommand(Guid Id) : IRequest<DeleteICSResponse>;

public sealed record DeleteICSResponse(Guid Id, string Message);
