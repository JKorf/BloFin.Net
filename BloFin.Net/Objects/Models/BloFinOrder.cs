using BloFin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Order info
    /// </summary>
    public record BloFinOrder
    {
        /// <summary>
        /// ["<c>orderId</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderId")]
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
        /// ["<c>instType</c>"] Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType? SymbolType { get; set; }
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
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>orderType</c>"] Order type
        /// </summary>
        [JsonPropertyName("orderType")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>reduceOnly</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// ["<c>state</c>"] Order status
        /// </summary>
        [JsonPropertyName("state")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>filledSize</c>"] Quantity filled
        /// </summary>
        [JsonPropertyName("filledSize")]
        public decimal QuantityFilled { get; set; }
        [JsonInclude, JsonPropertyName("filledAmount")]
        internal decimal QuantityFilled3
        {
            get => QuantityFilled;
            set => QuantityFilled = value;
        }
        /// <summary>
        /// ["<c>averagePrice</c>"] Average fill price
        /// </summary>
        [JsonPropertyName("averagePrice")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>pnl</c>"] Profit and loss
        /// </summary>
        [JsonPropertyName("pnl")]
        public decimal Pnl { get; set; }
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
        /// ["<c>orderCategory</c>"] Order category
        /// </summary>
        [JsonPropertyName("orderCategory")]
        public OrderCategory OrderCategory { get; set; }
        /// <summary>
        /// ["<c>tpTriggerPrice</c>"] Take profit trigger price
        /// </summary>
        [JsonPropertyName("tpTriggerPrice")]
        public decimal? TakeProfitTriggerPrice { get; set; }
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
        /// <summary>
        /// ["<c>tpOrderPrice</c>"] Take profit order price
        /// </summary>
        [JsonPropertyName("tpOrderPrice")]
        public decimal? TakeProfitOrderPrice { get; set; }
        /// <summary>
        /// ["<c>algoClientOrderId</c>"] Algo client order id
        /// </summary>
        [JsonPropertyName("algoClientOrderId")]
        public string? AlgoClientOrderId { get; set; }
        /// <summary>
        /// ["<c>cancelSource</c>"] Cancel source
        /// </summary>
        [JsonPropertyName("cancelSource")]
        public string? CancelSource { get; set; }
        /// <summary>
        /// ["<c>cancelSourceReason</c>"] Cancel source reason
        /// </summary>
        [JsonPropertyName("cancelSourceReason")]
        public string? CancelSourceReason { get; set; }
        /// <summary>
        /// ["<c>algoId</c>"] Algo id
        /// </summary>
        [JsonPropertyName("algoId")]
        public string? AlgoId { get; set; }
        /// <summary>
        /// ["<c>brokerId</c>"] Broker id
        /// </summary>
        [JsonPropertyName("brokerId")]
        public string? BrokerId { get; set; }
    }


}
