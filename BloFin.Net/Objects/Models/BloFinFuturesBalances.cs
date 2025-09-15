using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Futures account balance
    /// </summary>
    public record BloFinFuturesBalances
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Total equity
        /// </summary>
        [JsonPropertyName("totalEquity")]
        public decimal? TotalEquity { get; set; }
        /// <summary>
        /// Isolated equity
        /// </summary>
        [JsonPropertyName("isolatedEquity")]
        public decimal? IsolatedEquity { get; set; }
        /// <summary>
        /// Details
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
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Equity
        /// </summary>
        [JsonPropertyName("equity")]
        public decimal Equity { get; set; }
        /// <summary>
        /// Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Balance { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Isolated equity
        /// </summary>
        [JsonPropertyName("isolatedEquity")]
        public decimal IsolatedEquity { get; set; }
        /// <summary>
        /// Available
        /// </summary>
        [JsonPropertyName("available")]
        public decimal Available { get; set; }
        /// <summary>
        /// Available equity
        /// </summary>
        [JsonPropertyName("availableEquity")]
        public decimal AvailableEquity { get; set; }
        /// <summary>
        /// Frozen
        /// </summary>
        [JsonPropertyName("frozen")]
        public decimal Frozen { get; set; }
        /// <summary>
        /// Order frozen
        /// </summary>
        [JsonPropertyName("orderFrozen")]
        public decimal OrderFrozen { get; set; }
        /// <summary>
        /// Equity usd
        /// </summary>
        [JsonPropertyName("equityUsd")]
        public decimal EquityUsd { get; set; }
        /// <summary>
        /// Unrealized pnl
        /// </summary>
        [JsonPropertyName("unrealizedPnl")]
        public decimal? UnrealizedPnl { get; set; }
        /// <summary>
        /// Isolated unrealized pnl
        /// </summary>
        [JsonPropertyName("isolatedUnrealizedPnl")]
        public decimal IsolatedUnrealizedPnl { get; set; }
        /// <summary>
        /// Bonus
        /// </summary>
        [JsonPropertyName("bonus")]
        public decimal Bonus { get; set; }
    }


}
