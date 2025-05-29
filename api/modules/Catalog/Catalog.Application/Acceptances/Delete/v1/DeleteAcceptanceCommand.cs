using MediatR;
using System;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Delete.v1
{
    public sealed record DeleteAcceptanceCommand(Guid Id) : IRequest<DeleteAcceptanceResponse>;
}
