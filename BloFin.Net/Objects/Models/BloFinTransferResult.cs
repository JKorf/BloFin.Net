using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Transfer result
    /// </summary>
    public record BloFinTransferResult
    {
        /// <summary>
        /// ["<c>transferId</c>"] Transfer id
        /// </summary>
        [JsonPropertyName("transferId")]
        public string TransferId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientTransferId</c>"] Client transfer id
        /// </summary>
        [JsonPropertyName("clientTransferId")]
        public string? ClientTransferId { get; set; }
    }
}
