using AMIS.Framework.Core.Exceptions;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using System.Net;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;

public sealed class InvalidInspectionTransitionException : FshException
{
    public InvalidInspectionTransitionException(InspectionStatus from, InspectionStatus to)
        : base($"Cannot transition inspection from {from} to {to}.", [], HttpStatusCode.BadRequest)
    {
    }

    public InvalidInspectionTransitionException(Guid inspectionId, InspectionStatus from, InspectionStatus to)
        : base($"Cannot transition inspection {inspectionId} from {from} to {to}.", [], HttpStatusCode.BadRequest)
    {
    }
}
