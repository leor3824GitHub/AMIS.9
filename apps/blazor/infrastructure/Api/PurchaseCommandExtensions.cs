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
    }

    // Extends generated UpdatePurchaseCommand with client-side only properties
    public partial class UpdatePurchaseCommand
    {
        [JsonPropertyName("referenceNumber")]
        public string? ReferenceNumber { get; set; }

        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }
    }
}
