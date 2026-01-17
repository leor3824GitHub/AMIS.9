using MediatR;

namespace AMIS.WebApi.Inventories.Application.IssuanceItems.Delete.v1;
public sealed record DeleteIssuanceItemCommand(
    Guid Id) : IRequest;

