using System;
using System.Text.Json.Serialization;

namespace AMIS.Blazor.Infrastructure.Api
{
    // Extends generated CreatePurchaseCommand with client-side only properties
    public partial class CreatePurchaseCommand
    {
        [JsonPropertyName("referenceNumber")]
        public string? ReferenceNumber { get; set; }

        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }

        // Local-only identifier to support edit/clone flows; excluded from payload.
        [JsonIgnore]
        public Guid? Id { get; set; }

        // Client-side aggregate, kept out of the request body.
        [JsonIgnore]
        public double TotalAmount { get; set; }
    }

    // Extends generated UpdatePurchaseCommand with client-side only properties
    public partial class UpdatePurchaseCommand
    {
        [JsonPropertyName("referenceNumber")]
        public string? ReferenceNumber { get; set; }

        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }
    }

    // Client-side convenience property for purchase items to keep temporary ids.
    public partial class PurchaseItemDto
    {
        [JsonIgnore]
        public Guid? Id { get; set; }
    }
}
