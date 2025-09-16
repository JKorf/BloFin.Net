using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Transfer result
    /// </summary>
    public record BloFinTransferResult
    {
        /// <summary>
        /// Transfer id
        /// </summary>
        [JsonPropertyName("transferId")]
        public string TransferId { get; set; } = string.Empty;
        /// <summary>
        /// Client transfer id
        /// </summary>
        [JsonPropertyName("clientTransferId")]
        public string? ClientTransferId { get; set; }
    }
}
