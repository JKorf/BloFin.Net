using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Internal
{
    internal record BloFinSocketUpdate<T>
    {
        [JsonPropertyName("arg")]
        public Dictionary<string, string> Parameters { get; set; } = new();
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
        [JsonPropertyName("action")]
        public string? Action { get; set; }
    }
}
