using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Internal
{
    internal record BloFinSocketRequest
    {
        [JsonPropertyName("op")]
        public string Operation { get; set; } = string.Empty;
        [JsonPropertyName("args")]
        public Dictionary<string, string>[] Parameters { get; set; } = [];
    }
}
