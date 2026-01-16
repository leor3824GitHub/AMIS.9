using System;
using System.Text.Json.Serialization;

namespace AMIS.Blazor.Infrastructure.Api;

public partial class PurchaseItemDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}
