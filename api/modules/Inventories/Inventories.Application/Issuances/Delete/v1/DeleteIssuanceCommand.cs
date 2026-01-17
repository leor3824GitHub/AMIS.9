using MediatR;

namespace AMIS.WebApi.Inventories.Application.Issuances.Delete.v1;
public sealed record DeleteIssuanceCommand(
    Guid Id) : IRequest;

