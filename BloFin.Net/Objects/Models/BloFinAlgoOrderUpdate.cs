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
        /// ["<c>instType</c>"] Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tpslId</c>"] Tpsl order id
        /// </summary>
        [JsonPropertyName("tpslId")]
        public string TpslOrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>algoId</c>"] Algo order id
        /// </summary>
        [JsonPropertyName("algoId")]
        public string AlgoOrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>orderType</c>"] Order type
        /// </summary>
        [JsonPropertyName("orderType")]
        public AlgoOrderType OrderType { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>positionSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>marginMode</c>"] Margin mode
        /// </summary>
        [JsonPropertyName("marginMode")]
        public MarginMode MarginMode { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// ["<c>state</c>"] Status
        /// </summary>
        [JsonPropertyName("state")]
        public TpSlOrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>tpTriggerPrice</c>"] Tp trigger price
        /// </summary>
        [JsonPropertyName("tpTriggerPrice")]
        public decimal? TpTriggerPrice { get; set; }
        /// <summary>
        /// ["<c>tpOrderPrice</c>"] Tp order price
        /// </summary>
        [JsonPropertyName("tpOrderPrice")]
        public decimal? TpOrderPrice { get; set; }
        /// <summary>
        /// ["<c>slTriggerPrice</c>"] Sl trigger price
        /// </summary>
        [JsonPropertyName("slTriggerPrice")]
        public string? SlTriggerPrice { get; set; }
        /// <summary>
        /// ["<c>slOrderPrice</c>"] Sl order price
        /// </summary>
        [JsonPropertyName("slOrderPrice")]
        public string? SlOrderPrice { get; set; }
        /// <summary>
        /// ["<c>triggerPrice</c>"] Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public string? TriggerPrice { get; set; }
        /// <summary>
        /// ["<c>triggerPriceType</c>"] Trigger price type
        /// </summary>
        [JsonPropertyName("triggerPriceType")]
        public TriggerPriceType? TriggerPriceType { get; set; }
        /// <summary>
        /// ["<c>orderPrice</c>"] Order price
        /// </summary>
        [JsonPropertyName("orderPrice")]
        public string? OrderPrice { get; set; }
        /// <summary>
        /// ["<c>actualSize</c>"] Actual quantity
        /// </summary>
        [JsonPropertyName("actualSize")]
        public string? ActualQuantity { get; set; }
        /// <summary>
        /// ["<c>actualSide</c>"] Actual side
        /// </summary>
        [JsonPropertyName("actualSide")]
        public string? ActualSide { get; set; }
        /// <summary>
        /// ["<c>reduceOnly</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>cancelType</c>"] Cancel type
        /// </summary>
        [JsonPropertyName("cancelType")]
        public string? CancelType { get; set; }
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
        /// ["<c>brokerId</c>"] Broker id
        /// </summary>
        [JsonPropertyName("brokerId")]
        public string? BrokerId { get; set; }
        /// <summary>
        /// ["<c>attachAlgoOrders</c>"] Attach algo orders
        /// </summary>
        [JsonPropertyName("attachAlgoOrders")]
        public BloFinAlgoAttachedOrder[] AttachAlgoOrders { get; set; } = [];
    }
}
