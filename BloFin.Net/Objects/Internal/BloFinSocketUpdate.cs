using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Internal
{
    internal record BloFinSocketUpdate<T>
    {
        [JsonPropertyName("arg")]
        public Dictionary<string, object> Parameters { get; set; } = new();
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
        [JsonPropertyName("action")]
        public string? Action { get; set; }
    }
}
