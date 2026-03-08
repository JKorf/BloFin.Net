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
        /// ["<c>positionId</c>"] Position id
        /// </summary>
        [JsonPropertyName("positionId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>instType</c>"] Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// ["<c>marginMode</c>"] Margin mode
        /// </summary>
        [JsonPropertyName("marginMode")]
        public MarginMode MarginMode { get; set; }
        /// <summary>
        /// ["<c>positionSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>adl</c>"] Adl
        /// </summary>
        [JsonPropertyName("adl")]
        public decimal Adl { get; set; }
        /// <summary>
        /// ["<c>positions</c>"] Position size
        /// </summary>
        [JsonPropertyName("positions")]
        public decimal PositionSize { get; set; }
        /// <summary>
        /// ["<c>availablePositions</c>"] Position size which can be closed
        /// </summary>
        [JsonPropertyName("availablePositions")]
        public decimal ClosableSize { get; set; }
        /// <summary>
        /// ["<c>averagePrice</c>"] Average open price
        /// </summary>
        [JsonPropertyName("averagePrice")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// ["<c>margin</c>"] Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal? Margin { get; set; }
        /// <summary>
        /// ["<c>markPrice</c>"] Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// ["<c>marginRatio</c>"] Margin ratio
        /// </summary>
        [JsonPropertyName("marginRatio")]
        public decimal MarginRatio { get; set; }
        /// <summary>
        /// ["<c>liquidationPrice</c>"] Liquidation price
        /// </summary>
        [JsonPropertyName("liquidationPrice")]
        public decimal? LiquidationPrice { get; set; }
        /// <summary>
        /// ["<c>unrealizedPnl</c>"] Unrealized profit and loss
        /// </summary>
        [JsonPropertyName("unrealizedPnl")]
        public decimal UnrealizedPnl { get; set; }
        /// <summary>
        /// ["<c>unrealizedPnlRatio</c>"] Unrealized profit and loss ratio
        /// </summary>
        [JsonPropertyName("unrealizedPnlRatio")]
        public decimal UnrealizedPnlRatio { get; set; }
        /// <summary>
        /// ["<c>initialMargin</c>"] Initial margin
        /// </summary>
        [JsonPropertyName("initialMargin")]
        public decimal? InitialMargin { get; set; }
        /// <summary>
        /// ["<c>maintenanceMargin</c>"] Maintenance margin
        /// </summary>
        [JsonPropertyName("maintenanceMargin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        /// ["<c>createTime</c>"] Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>updateTime</c>"] Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
    }


}
