using System.Text.Json.Serialization;

namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PurchaseItemAcceptanceStatus
{
    Pending,
    Accepted,
    Rejected,
    PartiallyAccepted
}


