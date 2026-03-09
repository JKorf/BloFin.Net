using BloFin.Net.Enums;
using System.Text.Json.Serialization;
using System;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Position history
    /// </summary>
    public record BloFinPositionHistory
    {
        /// <summary>
        /// ["<c>historyId</c>"] History id
        /// </summary>
        [JsonPropertyName("historyId")]
        public long HistoryId { get; set; }
        /// <summary>
        /// ["<c>positionId</c>"] Position id
        /// </summary>
        [JsonPropertyName("positionId")]
        public long PositionId { get; set; }
        /// <summary>
        /// ["<c>instId</c>"] Symbol
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>instType</c>"] Inst type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType InstType { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
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
        /// ["<c>closePositions</c>"] Close positions
        /// </summary>
        [JsonPropertyName("closePositions")]
        public decimal ClosePositions { get; set; }
        /// <summary>
        /// ["<c>maxPositions</c>"] Max positions
        /// </summary>
        [JsonPropertyName("maxPositions")]
        public decimal MaxPositions { get; set; }
        /// <summary>
        /// ["<c>liquidationPositions</c>"] Liquidation positions
        /// </summary>
        [JsonPropertyName("liquidationPositions")]
        public decimal LiquidationPositions { get; set; }
        /// <summary>
        /// ["<c>openAveragePrice</c>"] Open average price
        /// </summary>
        [JsonPropertyName("openAveragePrice")]
        public decimal OpenAveragePrice { get; set; }
        /// <summary>
        /// ["<c>closeAveragePrice</c>"] Close average price
        /// </summary>
        [JsonPropertyName("closeAveragePrice")]
        public decimal CloseAveragePrice { get; set; }
        /// <summary>
        /// ["<c>createTime</c>"] Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>updateTime</c>"] Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// ["<c>realizedPnl</c>"] Realized profit and loss
        /// </summary>
        [JsonPropertyName("realizedPnl")]
        public decimal RealizedPnl { get; set; }
        /// <summary>
        /// ["<c>realizedPnlRatio</c>"] Realized profit and loss ratio
        /// </summary>
        [JsonPropertyName("realizedPnlRatio")]
        public decimal RealizedPnlRatio { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
    }


}
