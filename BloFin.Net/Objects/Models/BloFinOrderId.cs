using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// TP/SL order id
    /// </summary>
    public record BloFinTpSlOrderId
    {
        /// <summary>
        /// ["<c>tpslId</c>"] Tpsl order id
        /// </summary>
        [JsonPropertyName("tpslId")]
        public string TpslOrderId { get; set; } = string.Empty;
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
