using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Internal
{
    internal record BloFinResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Message { get; set; } = string.Empty;
    }

    internal record BloFinResponse<T> : BloFinResponse
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
