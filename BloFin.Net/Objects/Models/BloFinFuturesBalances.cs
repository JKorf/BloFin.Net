using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Futures account balance
    /// </summary>
    public record BloFinFuturesBalances
    {
        /// <summary>
        /// ["<c>ts</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>totalEquity</c>"] Total equity
        /// </summary>
        [JsonPropertyName("totalEquity")]
        public decimal? TotalEquity { get; set; }
        /// <summary>
        /// ["<c>isolatedEquity</c>"] Isolated equity
        /// </summary>
        [JsonPropertyName("isolatedEquity")]
        public decimal? IsolatedEquity { get; set; }
        /// <summary>
        /// ["<c>details</c>"] Details
        /// </summary>
        [JsonPropertyName("details")]
        public BloFinFuturesBalance[] Details { get; set; } = [];
    }

    /// <summary>
    /// Balance update
    /// </summary>
    public record BloFinFuturesBalance
    {
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>equity</c>"] Equity
        /// </summary>
        [JsonPropertyName("equity")]
        public decimal Equity { get; set; }
        /// <summary>
        /// ["<c>balance</c>"] Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
        /// <summary>
        /// ["<c>ts</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>isolatedEquity</c>"] Isolated equity
        /// </summary>
        [JsonPropertyName("isolatedEquity")]
        public decimal IsolatedEquity { get; set; }
        /// <summary>
        /// ["<c>available</c>"] Available
        /// </summary>
        [JsonPropertyName("available")]
        public decimal Available { get; set; }
        /// <summary>
        /// ["<c>availableEquity</c>"] Available equity
        /// </summary>
        [JsonPropertyName("availableEquity")]
        public decimal AvailableEquity { get; set; }
        /// <summary>
        /// ["<c>frozen</c>"] Frozen
        /// </summary>
        [JsonPropertyName("frozen")]
        public decimal Frozen { get; set; }
        /// <summary>
        /// ["<c>orderFrozen</c>"] Order frozen
        /// </summary>
        [JsonPropertyName("orderFrozen")]
        public decimal OrderFrozen { get; set; }
        /// <summary>
        /// ["<c>equityUsd</c>"] Equity usd
        /// </summary>
        [JsonPropertyName("equityUsd")]
        public decimal EquityUsd { get; set; }
        /// <summary>
        /// ["<c>unrealizedPnl</c>"] Unrealized pnl
        /// </summary>
        [JsonPropertyName("unrealizedPnl")]
        public decimal? UnrealizedPnl { get; set; }
        /// <summary>
        /// ["<c>isolatedUnrealizedPnl</c>"] Isolated unrealized pnl
        /// </summary>
        [JsonPropertyName("isolatedUnrealizedPnl")]
        public decimal IsolatedUnrealizedPnl { get; set; }
        /// <summary>
        /// ["<c>bonus</c>"] Bonus
        /// </summary>
        [JsonPropertyName("bonus")]
        public decimal Bonus { get; set; }
    }


}
