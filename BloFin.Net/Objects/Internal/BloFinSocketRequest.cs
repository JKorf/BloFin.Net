using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Internal
{
    internal record BloFinSocketRequest
    {
        [JsonPropertyName("op")]
        public string Operation { get; set; } = string.Empty;
        [JsonPropertyName("args")]
        public Dictionary<string, object>[] Parameters { get; set; } = [];
    }
}
