using AMIS.Framework.Core.Exceptions;
using System.Net;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;

public sealed class AcceptanceValidationException : FshException
{
    public AcceptanceValidationException(string message)
        : base(message, [], HttpStatusCode.BadRequest)
    {
    }

    public static AcceptanceValidationException ForInspectionNotLinkedToPurchase()
        => new("The specified inspection is not linked to a purchase; cannot create acceptance.");

    public static AcceptanceValidationException ForMissingInspectionRequest()
        => new("Submit an inspection request before recording an acceptance.");

    public static AcceptanceValidationException ForInspectionNotCompleted()
        => new("Complete the inspection before recording an acceptance.");

    public static AcceptanceValidationException ForMissingInspection()
        => new("Record an inspection for the purchase before creating an acceptance.");
}
