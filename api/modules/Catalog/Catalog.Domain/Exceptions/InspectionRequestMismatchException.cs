using AMIS.Framework.Core.Exceptions;
using System.Net;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;

public sealed class InspectionRequestMismatchException : FshException
{
    public InspectionRequestMismatchException()
        : base("The inspection must target the same purchase as its inspection request.", [], HttpStatusCode.BadRequest)
    {
    }

    public InspectionRequestMismatchException(Guid inspectionRequestId, Guid expectedPurchaseId, Guid providedPurchaseId)
        : base($"Inspection request {inspectionRequestId} is linked to purchase {expectedPurchaseId}, but inspection targets purchase {providedPurchaseId}.", 
               [], HttpStatusCode.BadRequest)
    {
    }
}
