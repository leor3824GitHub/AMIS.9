using MediatR;
using System;

namespace AMIS.WebApi.Catalog.Application.Inspections.Delete.v1
{
    public sealed record DeleteInspectionCommand(Guid Id) : IRequest<DeleteInspectionResponse>;
}
