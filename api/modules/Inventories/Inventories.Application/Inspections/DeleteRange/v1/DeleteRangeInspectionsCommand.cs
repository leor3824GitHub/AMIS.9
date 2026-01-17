using MediatR;
using System.Collections.Generic;

namespace AMIS.WebApi.Inventories.Application.Inspections.DeleteRange.v1
{
    public record DeleteRangeInspectionsCommand(IEnumerable<Guid> InspectionIds) : IRequest;
}

