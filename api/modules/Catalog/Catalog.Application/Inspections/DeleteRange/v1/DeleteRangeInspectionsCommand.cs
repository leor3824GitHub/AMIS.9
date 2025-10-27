using MediatR;
using System.Collections.Generic;

namespace AMIS.WebApi.Catalog.Application.Inspections.DeleteRange.v1
{
    public record DeleteRangeInspectionsCommand(IEnumerable<Guid> InspectionIds) : IRequest;
}
