using System.Text.Json.Serialization;

namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Represents the possible states of an AssetAssignmentHistory record.
/// Matches the pattern used by AcceptanceStatus for consistency across the domain model.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AssignmentStatus
{
    Active = 0,      // Asset currently assigned to employee
    Returned = 1,    // Asset returned from employee
    Transferred = 2  // Asset transferred to another employee
}
