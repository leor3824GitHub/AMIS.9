using AMIS.Framework.Core.Exceptions;
using System.Net;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;

public sealed class InvalidInspectionStatusException : FshException
{
    public InvalidInspectionStatusException(string message)
        : base(message, [], HttpStatusCode.BadRequest)
    {
    }

    public InvalidInspectionStatusException(Guid inspectionId, string currentStatus, string action)
        : base($"Cannot {action} inspection {inspectionId} with status '{currentStatus}'.", [], HttpStatusCode.BadRequest)
    {
    }
}
