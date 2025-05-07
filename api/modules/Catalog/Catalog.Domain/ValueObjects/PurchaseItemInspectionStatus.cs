using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PurchaseItemInspectionStatus
{
    NotInspected,
    Passed,
    Failed,
    PartiallyPassed
}
