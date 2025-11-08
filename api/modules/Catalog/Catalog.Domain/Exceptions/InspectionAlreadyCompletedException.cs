using AMIS.Framework.Core.Exceptions;
using System.Net;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;

public sealed class InspectionAlreadyCompletedException : FshException
{
    public InspectionAlreadyCompletedException()
        : base("This inspection request has already been completed. Create a new request for additional inspections.", 
               [], HttpStatusCode.Conflict)
    {
    }

    public InspectionAlreadyCompletedException(Guid inspectionRequestId)
        : base($"Inspection request {inspectionRequestId} has already been completed. Create a new request for additional inspections.", 
               [], HttpStatusCode.Conflict)
    {
    }
}
