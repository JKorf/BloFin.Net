using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Algo order result
    /// </summary>
    public record BloFinAlgoOrderId
    {
        /// <summary>
        /// ["<c>algoId</c>"] Algo order id
        /// </summary>
        [JsonPropertyName("algoId")]
        public string AlgoOrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>code</c>"] Code
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }
        /// <summary>
        /// ["<c>msg</c>"] Message
        /// </summary>
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
    }


}
