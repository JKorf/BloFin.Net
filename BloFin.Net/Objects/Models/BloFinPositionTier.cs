using BloFin.Net.Enums;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Position tier
    /// </summary>
    public record BloFinPositionTier
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>marginMode</c>"] Margin mode
        /// </summary>
        [JsonPropertyName("marginMode")]
        public MarginMode MarginMode { get; set; }
        /// <summary>
        /// ["<c>minSize</c>"] Min size
        /// </summary>
        [JsonPropertyName("minSize")]
        public decimal? MinSize { get; set; }
        /// <summary>
        /// ["<c>maxSize</c>"] Max size
        /// </summary>
        [JsonPropertyName("maxSize")]
        public decimal? MaxSize { get; set; }
        /// <summary>
        /// ["<c>maintenanceMarginRate</c>"] Maintenance margin rate
        /// </summary>
        [JsonPropertyName("maintenanceMarginRate")]
        public decimal? MaintenanceMarginRate { get; set; }
        /// <summary>
        /// ["<c>maxLeverage</c>"] Max leverage
        /// </summary>
        [JsonPropertyName("maxLeverage")]
        public decimal? MaxLeverage { get; set; }
    }
}
