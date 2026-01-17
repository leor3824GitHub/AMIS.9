using MediatR;
using System;

namespace AMIS.WebApi.Inventories.Application.Inspections.Delete.v1
{
    public sealed record DeleteInspectionCommand(Guid Id) : IRequest<DeleteInspectionResponse>;
}

