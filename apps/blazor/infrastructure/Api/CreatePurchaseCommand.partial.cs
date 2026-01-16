using System.Text.Json.Serialization;

namespace AMIS.Blazor.Infrastructure.Api;

// Extends the generated CreatePurchaseCommand with client-only fields used by the UI.
public partial class CreatePurchaseCommand
{
    [JsonPropertyName("referenceNumber")]
    public string? ReferenceNumber { get; set; }

    [JsonPropertyName("totalAmount")]
    public double TotalAmount { get; set; }

    [JsonPropertyName("remarks")]
    public string? Remarks { get; set; }

    [JsonPropertyName("id")]
    public Guid? Id { get; set; }
}
