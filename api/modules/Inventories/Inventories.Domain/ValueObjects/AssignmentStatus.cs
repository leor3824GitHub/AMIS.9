using System.Text.Json.Serialization;

namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Enumeration for asset assignment status
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AssignmentStatus
{
    Active = 0,
    Returned = 1,
    Transferred = 2
}
