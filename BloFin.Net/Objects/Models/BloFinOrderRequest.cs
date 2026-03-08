using BloFin.Net.Enums;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Order request
    /// </summary>
    public record BloFinOrderRequest
    {
        /// <summary>
        /// ["<c>instId</c>"] Symbol name, for example `ETH-USDT`
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
        [JsonPropertyName("positionSide"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PositionSide? PositionSide { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide OrderSide { get; set; }
        /// <summary>
        /// ["<c>orderType</c>"] Order type
        /// </summary>
        [JsonPropertyName("orderType")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Order price
        /// </summary>
        [JsonPropertyName("price"), JsonConverter(typeof(DecimalStringWriterConverter)), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Quantity
        /// </summary>
        [JsonPropertyName("size"), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>reduceOnly</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>clientOrderId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>tpTriggerPrice</c>"] Take profit trigger price
        /// </summary>
        [JsonPropertyName("tpTriggerPrice"), JsonConverter(typeof(DecimalStringWriterConverter)), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? TakeProfitTriggerPrice { get; set; }
        /// <summary>
        /// ["<c>tpOrderPrice</c>"] Take profit order price
        /// </summary>
        [JsonPropertyName("tpOrderPrice"), JsonConverter(typeof(DecimalStringWriterConverter)), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? TakeProfitOrderPrice { get; set; }
        /// <summary>
        /// ["<c>slTriggerPrice</c>"] Stop loss trigger price
        /// </summary>
        [JsonPropertyName("slTriggerPrice"), JsonConverter(typeof(DecimalStringWriterConverter)), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? StopLossTriggerPrice { get; set; }
        /// <summary>
        /// ["<c>slOrderPrice</c>"] Stop loss order price
        /// </summary>
        [JsonPropertyName("slOrderPrice"), JsonConverter(typeof(DecimalStringWriterConverter)), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? StopLossOrderPrice { get; set; }
        /// <summary>
        /// ["<c>brokerId</c>"] Broker id
        /// </summary>
        [JsonPropertyName("brokerId")]
        internal string? BrokerId { get; set; }
    }
}
