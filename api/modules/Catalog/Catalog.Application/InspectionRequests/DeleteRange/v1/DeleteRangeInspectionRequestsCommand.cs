using MediatR;
using System.Collections.Generic;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.DeleteRange.v1
{
    public record DeleteRangeInspectionRequestsCommand(IEnumerable<Guid> InspectionRequestIds) : IRequest;
}
