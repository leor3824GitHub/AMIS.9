using MediatR;
using System;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Delete.v1
{
    public sealed record DeleteAcceptanceItemCommand(Guid Id) : IRequest<DeleteAcceptanceItemResponse>;
}
