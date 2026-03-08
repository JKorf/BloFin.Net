using BloFin.Net.Enums;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Close position result
    /// </summary>
    public record BloFinClosePositionResult
    {
        /// <summary>
        /// ["<c>instId</c>"] Inst id
        /// </summary>
        [JsonPropertyName("instId")]
        public string InstId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>positionSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>clientOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
    }


}
