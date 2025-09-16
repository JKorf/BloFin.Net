using System.Text.Json.Serialization;

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
