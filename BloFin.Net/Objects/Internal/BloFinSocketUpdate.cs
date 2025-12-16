using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Internal
{
    internal record BloFinSocketUpdate
    {
        [JsonPropertyName("arg")]
        public Dictionary<string, string> Parameters { get; set; } = new();
        [JsonPropertyName("action")]
        public string? Action { get; set; }
    }

    internal record BloFinSocketUpdate<T>: BloFinSocketUpdate
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
