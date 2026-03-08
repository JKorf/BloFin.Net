using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Ticker info
    /// </summary>
    public record BloFinTicker
    {
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>last</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>lastSize</c>"] Last trade quantity
        /// </summary>
        [JsonPropertyName("lastSize")]
        public decimal LastQuantity { get; set; }
        /// <summary>
        /// ["<c>askPrice</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("askPrice")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>askSize</c>"] Best ask quantity
        /// </summary>
        [JsonPropertyName("askSize")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// ["<c>bidPrice</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("bidPrice")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>bidSize</c>"] Best bid quantity
        /// </summary>
        [JsonPropertyName("bidSize")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>high24h</c>"] Highest price in last 24h
        /// </summary>
        [JsonPropertyName("high24h")]
        public decimal High24h { get; set; }
        /// <summary>
        /// ["<c>open24h</c>"] Open price 24h ago
        /// </summary>
        [JsonPropertyName("open24h")]
        public decimal Open24h { get; set; }
        /// <summary>
        /// ["<c>low24h</c>"] Lowest price in last 24h
        /// </summary>
        [JsonPropertyName("low24h")]
        public decimal Low24h { get; set; }
        /// <summary>
        /// ["<c>volCurrency24h</c>"] Volume in the last 24h in base asset
        /// </summary>
        [JsonPropertyName("volCurrency24h")]
        public decimal BaseVolume24h { get; set; }
        /// <summary>
        /// ["<c>vol24h</c>"] Volume in the last 24h in contracts
        /// </summary>
        [JsonPropertyName("vol24h")]
        public decimal ContractVolume24h { get; set; }
        /// <summary>
        /// ["<c>ts</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
    }


}
