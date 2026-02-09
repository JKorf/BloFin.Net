using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Internal
{
    internal record BloFinSocketResponse
    {
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
        [JsonPropertyName("arg")]
        public Dictionary<string, string> Parameters { get; set; } = new();
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
    }
}
