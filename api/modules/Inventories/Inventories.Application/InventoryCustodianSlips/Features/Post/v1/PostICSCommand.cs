using MediatR;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Features.Post.v1;

public sealed record PostICSCommand(Guid Id) : IRequest<PostICSResponse>;

public sealed record PostICSResponse(Guid Id, string Message);
