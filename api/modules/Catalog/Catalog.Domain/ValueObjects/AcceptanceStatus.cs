using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AcceptanceStatus
{
    Pending = 0,        // Items received, awaiting inspection/acceptance decision
    Inspected = 1,      // Inspector completed verification
    Accepted = 2,       // All items accepted into inventory
    PartiallyAccepted = 3,  // Some items accepted, some rejected
    Rejected = 4,       // Items rejected
    Posted = 5,         // Recorded in asset register
    Cancelled = 6       // Acceptance cancelled
}
