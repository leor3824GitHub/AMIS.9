using AMIS.Framework.Core.Exceptions;
using System.Net;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;

public sealed class InspectionValidationException : FshException
{
    public InspectionValidationException(string message)
        : base(message, [], HttpStatusCode.BadRequest)
    {
    }

    public InspectionValidationException(string message, IEnumerable<string> errors)
        : base(message, errors, HttpStatusCode.BadRequest)
    {
    }

    public static InspectionValidationException ForInvalidScheduledDate()
        => new("Scheduled date cannot be in the past.");

    public static InspectionValidationException ForMissingConditions()
        => new("Conditions must be specified for conditional approval.");

    public static InspectionValidationException ForMissingPartialDetails()
        => new("Partial details must be specified for partial approval.");

    public static InspectionValidationException ForMissingHoldReason()
        => new("A reason must be provided to put inspection on hold.");

    public static InspectionValidationException ForMissingQuarantineReason()
        => new("A reason must be provided to quarantine the inspection.");

    public static InspectionValidationException ForMissingReInspectionReason()
        => new("A reason must be provided for requiring re-inspection.");
}
