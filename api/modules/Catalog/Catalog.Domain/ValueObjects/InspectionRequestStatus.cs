using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InspectionRequestStatus
{
    Pending = 0,   // Request has been created but not yet assigned.
    Assigned = 1,  // Inspector has been assigned to the request.
    InProgress = 2, // Inspection is currently ongoing.
    Completed = 3, // Inspection has been completed.
    Failed = 4,     // Inspection completed with at least one failed item.
    Accepted = 5    // Acceptance has been posted for the inspected purchase.
}

