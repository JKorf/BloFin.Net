using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Internal
{
    internal class BloFinSocketLoginRequest
    {
        [JsonPropertyName("apiKey")]
        public string ApiKey { get; set; } = string.Empty;
        [JsonPropertyName("passphrase")]
        public string Passphrase { get; set; } = string.Empty;
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = string.Empty;
        [JsonPropertyName("sign")]
        public string Sign { get; set; } = string.Empty;
        [JsonPropertyName("nonce")]
        public string Nonce { get; set; } = string.Empty;
    }
}
