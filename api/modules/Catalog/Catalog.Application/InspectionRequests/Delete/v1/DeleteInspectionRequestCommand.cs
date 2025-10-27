using MediatR;
using System;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Delete.v1
{
    public sealed record DeleteInspectionRequestCommand(Guid Id) : IRequest<DeleteInspectionRequestResponse>;
}
