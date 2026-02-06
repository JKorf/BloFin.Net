using BloFin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Position info
    /// </summary>
    public record BloFinPosition
    {
        /// <summary>
        /// Position id
        /// </summary>
        [JsonPropertyName("positionId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// Margin mode
        /// </summary>
        [JsonPropertyName("marginMode")]
        public MarginMode MarginMode { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Adl
        /// </summary>
        [JsonPropertyName("adl")]
        public decimal Adl { get; set; }
        /// <summary>
        /// Position size
        /// </summary>
        [JsonPropertyName("positions")]
        public decimal PositionSize { get; set; }
        /// <summary>
        /// Position size which can be closed
        /// </summary>
        [JsonPropertyName("availablePositions")]
        public decimal ClosableSize { get; set; }
        /// <summary>
        /// Average open price
        /// </summary>
        [JsonPropertyName("averagePrice")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal? Margin { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// Margin ratio
        /// </summary>
        [JsonPropertyName("marginRatio")]
        public decimal MarginRatio { get; set; }
        /// <summary>
        /// Liquidation price
        /// </summary>
        [JsonPropertyName("liquidationPrice")]
        public decimal? LiquidationPrice { get; set; }
        /// <summary>
        /// Unrealized profit and loss
        /// </summary>
        [JsonPropertyName("unrealizedPnl")]
        public decimal UnrealizedPnl { get; set; }
        /// <summary>
        /// Unrealized profit and loss ratio
        /// </summary>
        [JsonPropertyName("unrealizedPnlRatio")]
        public decimal UnrealizedPnlRatio { get; set; }
        /// <summary>
        /// Initial margin
        /// </summary>
        [JsonPropertyName("initialMargin")]
        public decimal? InitialMargin { get; set; }
        /// <summary>
        /// Maintenance margin
        /// </summary>
        [JsonPropertyName("maintenanceMargin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
    }


}
