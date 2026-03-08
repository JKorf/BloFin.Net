using BloFin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Algo order info
    /// </summary>
    public record BloFinAlgoOrder
    {
        /// <summary>
        /// ["<c>algoId</c>"] Algo order id
        /// </summary>
        [JsonPropertyName("algoId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
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
        /// ["<c>side</c>"] Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
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
        /// ["<c>actualSize</c>"] Actual quantity
        /// </summary>
        [JsonPropertyName("actualSize")]
        public decimal? ActualQuantity { get; set; }
        /// <summary>
        /// ["<c>triggerPrice</c>"] Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// ["<c>triggerPriceType</c>"] Trigger price type
        /// </summary>
        [JsonPropertyName("triggerPriceType")]
        public TriggerPriceType? TriggerPriceType { get; set; }
        /// <summary>
        /// ["<c>brokerId</c>"] Broker id
        /// </summary>
        [JsonPropertyName("brokerId")]
        public string? BrokerId { get; set; }
        /// <summary>
        /// ["<c>createTime</c>"] Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>attachAlgoOrders</c>"] Attach algo orders
        /// </summary>
        [JsonPropertyName("attachAlgoOrders")]
        public BloFinAlgoAttachedOrder[] AttachAlgoOrders { get; set; } = [];
    }

    /// <summary>
    /// Attached order info
    /// </summary>
    public record BloFinAlgoAttachedOrder
    {
        /// <summary>
        /// ["<c>tpTriggerPrice</c>"] Take profit trigger price
        /// </summary>
        [JsonPropertyName("tpTriggerPrice")]
        public decimal? TakeProfitTriggerPrice { get; set; }
        /// <summary>
        /// ["<c>tpOrderPrice</c>"] Take profit order price
        /// </summary>
        [JsonPropertyName("tpOrderPrice")]
        public decimal? TakeProfitOrderPrice { get; set; }
        /// <summary>
        /// ["<c>tpTriggerPriceType</c>"] Take profit trigger price type
        /// </summary>
        [JsonPropertyName("tpTriggerPriceType")]
        public TriggerPriceType? TakeProfitTriggerPriceType { get; set; }
        /// <summary>
        /// ["<c>slTriggerPrice</c>"] Stop loss trigger price
        /// </summary>
        [JsonPropertyName("slTriggerPrice")]
        public decimal? StopLossTriggerPrice { get; set; }
        /// <summary>
        /// ["<c>slOrderPrice</c>"] Stop loss order price
        /// </summary>
        [JsonPropertyName("slOrderPrice")]
        public decimal? StopLossOrderPrice { get; set; }
        /// <summary>
        /// ["<c>slTriggerPriceType</c>"] Stop loss trigger price type
        /// </summary>
        [JsonPropertyName("slTriggerPriceType")]
        public TriggerPriceType? StopLossTriggerPriceType { get; set; }
    } 


}
