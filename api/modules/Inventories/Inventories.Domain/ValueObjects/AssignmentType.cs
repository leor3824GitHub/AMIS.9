using System.Text.Json.Serialization;

namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Tracks the type of asset assignment action
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AssignmentType
{
    /// <summary>Initial assignment of asset to employee (ICS or PAR issuance)</summary>
    Initial = 0,

    /// <summary>Transfer of asset to another employee</summary>
    Transfer = 1,

    /// <summary>Return of asset by employee</summary>
    Return = 2,

    /// <summary>Issue of asset to employee (formal issuance)</summary>
    Issue = 3
}
