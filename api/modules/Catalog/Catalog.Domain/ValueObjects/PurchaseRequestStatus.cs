using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PurchaseRequestStatus
{
    Draft,          // The purchase request has been created but is not yet submitted for approval
    Submitted,      // The purchase request has been submitted for approval
    Approved,       // The purchase request has been approved by the designated authority
    Rejected,       // The purchase request has been rejected
    Cancelled       // The purchase request has been cancelled before approval
}
