using MediatR;
using System.Collections.Generic;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.DeleteRange.v1
{
    public record DeleteRangeInspectionRequestsCommand(IEnumerable<Guid> InspectionRequestIds) : IRequest;
}

