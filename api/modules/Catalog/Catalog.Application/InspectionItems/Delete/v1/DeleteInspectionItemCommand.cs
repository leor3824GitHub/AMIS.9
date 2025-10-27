using MediatR;
using System;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Delete.v1
{
    public sealed record DeleteInspectionItemCommand(Guid Id) : IRequest<DeleteInspectionItemResponse>;
}
