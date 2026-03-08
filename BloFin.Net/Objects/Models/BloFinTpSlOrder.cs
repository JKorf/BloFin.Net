using BloFin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// TP/SL order info
    /// </summary>
    public record BloFinTpSlOrder
    {
        /// <summary>
        /// ["<c>tpslId</c>"] Tpsl order id
        /// </summary>
        [JsonPropertyName("tpslId")]
        public string OrderId { get; set; } = string.Empty;
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
        public decimal? SlTriggerPrice { get; set; }
        /// <summary>
        /// ["<c>slOrderPrice</c>"] Sl order price
        /// </summary>
        [JsonPropertyName("slOrderPrice")]
        public decimal? SlOrderPrice { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// ["<c>state</c>"] Status
        /// </summary>
        [JsonPropertyName("state")]
        public TpSlOrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>leverage</c>"] Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// ["<c>reduceOnly</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>actualSize</c>"] Actual quantity
        /// </summary>
        [JsonPropertyName("actualSize")]
        public decimal? ActualQuantity { get; set; }
        /// <summary>
        /// ["<c>clientOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>orderCategory</c>"] Order category
        /// </summary>
        [JsonPropertyName("orderCategory")]
        public OrderCategory? OrderCategory { get; set; }
        /// <summary>
        /// ["<c>createTime</c>"] Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>brokerId</c>"] Broker id
        /// </summary>
        [JsonPropertyName("brokerId")]
        public string? BrokerId { get; set; }
    }


}
