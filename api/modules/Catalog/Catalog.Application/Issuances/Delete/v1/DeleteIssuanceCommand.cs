using MediatR;

namespace AMIS.WebApi.Catalog.Application.Issuances.Delete.v1;
public sealed record DeleteIssuanceCommand(
    Guid Id) : IRequest;
