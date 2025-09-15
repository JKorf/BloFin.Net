using BloFin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// TP/SL order info
    /// </summary>
    public record BloFinTpSlOrder
    {
        /// <summary>
        /// Tpsl order id
        /// </summary>
        [JsonPropertyName("tpslId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
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
        /// Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Tp trigger price
        /// </summary>
        [JsonPropertyName("tpTriggerPrice")]
        public decimal? TpTriggerPrice { get; set; }
        /// <summary>
        /// Tp order price
        /// </summary>
        [JsonPropertyName("tpOrderPrice")]
        public decimal? TpOrderPrice { get; set; }
        /// <summary>
        /// Sl trigger price
        /// </summary>
        [JsonPropertyName("slTriggerPrice")]
        public decimal? SlTriggerPrice { get; set; }
        /// <summary>
        /// Sl order price
        /// </summary>
        [JsonPropertyName("slOrderPrice")]
        public decimal? SlOrderPrice { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("state")]
        public TpSlOrderStatus Status { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// Actual quantity
        /// </summary>
        [JsonPropertyName("actualSize")]
        public decimal? ActualQuantity { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Order category
        /// </summary>
        [JsonPropertyName("orderCategory")]
        public OrderCategory? OrderCategory { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Broker id
        /// </summary>
        [JsonPropertyName("brokerId")]
        public string? BrokerId { get; set; }
    }


}
