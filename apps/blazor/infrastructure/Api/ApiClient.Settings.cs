using System.Text.Json;
using System.Text.Json.Serialization;

namespace AMIS.Blazor.Infrastructure.Api;

public partial class ApiClient
{
    static partial void UpdateJsonSerializerSettings(JsonSerializerOptions settings)
    {
        // Skip serializing default values (e.g., enum = 0) so optional filters are omitted.
        // This prevents sending Status=0 (NotReceived) when no status filter is intended.
        settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    }
}
