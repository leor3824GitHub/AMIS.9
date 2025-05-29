using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InspectionRequestStatus
{
    Pending,   // Request has been created but not yet assigned.
    Assigned,  // Inspector has been assigned to the request.
    InProgress, // Inspection is currently ongoing.
    Completed, // Inspection has been completed.
    Cancelled  // The request was cancelled.
}

