using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InspectionItemStatus
{
    NotInspected,
    Passed,
    Failed,
    Partial,
    Rejected,
    AcceptedWithDeviation
}


