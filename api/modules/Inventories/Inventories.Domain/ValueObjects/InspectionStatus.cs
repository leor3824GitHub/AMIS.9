using System.Text.Json.Serialization;

namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InspectionStatus
{
    InProgress,
    Completed,
    Approved,
    Rejected,
    Cancelled
}

