using MediatR;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Delete.v1;
public sealed record DeleteIssuanceItemCommand(
    Guid Id) : IRequest;
