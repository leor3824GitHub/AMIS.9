using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmploymentStatus
{
    Active,         // Currently employed and working
    Inactive,       // Temporarily not working (on leave, suspended)
    Terminated,     // Employment ended
    Retired         // Retired from service
}
