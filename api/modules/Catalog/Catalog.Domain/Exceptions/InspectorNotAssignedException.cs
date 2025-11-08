using AMIS.Framework.Core.Exceptions;
using System.Net;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;

public sealed class InspectorNotAssignedException : FshException
{
    public InspectorNotAssignedException()
        : base("Assign an inspector to the request before creating an inspection.", [], HttpStatusCode.BadRequest)
    {
    }

    public InspectorNotAssignedException(Guid inspectionRequestId)
        : base($"Inspection request {inspectionRequestId} must have an inspector assigned before creating an inspection.", 
               [], HttpStatusCode.BadRequest)
    {
    }
}
