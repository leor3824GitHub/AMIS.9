using AMIS.Framework.Core.Exceptions;
using System.Net;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;

public sealed class InspectionRequestRequiredException : FshException
{
    public InspectionRequestRequiredException()
        : base("Create an inspection request before recording an inspection.", [], HttpStatusCode.BadRequest)
    {
    }

    public InspectionRequestRequiredException(string message)
        : base(message, [], HttpStatusCode.BadRequest)
    {
    }
}
