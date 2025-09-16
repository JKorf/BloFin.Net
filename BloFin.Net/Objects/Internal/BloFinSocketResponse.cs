using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Internal
{
    internal record BloFinSocketResponse
    {
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
        [JsonPropertyName("args")]
        public Dictionary<string, object> Parameters { get; set; } = new();
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
    }
}
