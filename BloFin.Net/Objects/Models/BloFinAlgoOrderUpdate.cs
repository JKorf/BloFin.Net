using BloFin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Algo order update
    /// </summary>
    public record BloFinAlgoOrderUpdate
    {
        /// <summary>
        /// Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Tpsl order id
        /// </summary>
        [JsonPropertyName("tpslId")]
        public string TpslOrderId { get; set; } = string.Empty;
        /// <summary>
        /// Algo order id
        /// </summary>
        [JsonPropertyName("algoId")]
        public string AlgoOrderId { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("orderType")]
        public AlgoOrderType OrderType { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Margin mode
        /// </summary>
        [JsonPropertyName("marginMode")]
        public MarginMode MarginMode { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("state")]
        public TpSlOrderStatus Status { get; set; }
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
        public string? SlTriggerPrice { get; set; }
        /// <summary>
        /// Sl order price
        /// </summary>
        [JsonPropertyName("slOrderPrice")]
        public string? SlOrderPrice { get; set; }
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public string? TriggerPrice { get; set; }
        /// <summary>
        /// Trigger price type
        /// </summary>
        [JsonPropertyName("triggerPriceType")]
        public TriggerPriceType? TriggerPriceType { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("orderPrice")]
        public string? OrderPrice { get; set; }
        /// <summary>
        /// Actual quantity
        /// </summary>
        [JsonPropertyName("actualSize")]
        public string? ActualQuantity { get; set; }
        /// <summary>
        /// Actual side
        /// </summary>
        [JsonPropertyName("actualSide")]
        public string? ActualSide { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// Cancel type
        /// </summary>
        [JsonPropertyName("cancelType")]
        public string? CancelType { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// Broker id
        /// </summary>
        [JsonPropertyName("brokerId")]
        public string? BrokerId { get; set; }
        /// <summary>
        /// Attach algo orders
        /// </summary>
        [JsonPropertyName("attachAlgoOrders")]
        public BloFinAlgoAttachedOrder[] AttachAlgoOrders { get; set; } = [];
    }
}
