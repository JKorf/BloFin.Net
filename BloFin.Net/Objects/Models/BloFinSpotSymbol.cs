using BloFin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Spot instrument information
    /// </summary>
    public record BloFinSpotSymbol
    {
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>baseCurrency</c>"] Base asset
        /// </summary>
        [JsonPropertyName("baseCurrency")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quoteCurrency</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("quoteCurrency")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>contractValue</c>"] Contract size
        /// </summary>
        [JsonPropertyName("contractValue")]
        public decimal? ContractSize { get; set; }
        /// <summary>
        /// ["<c>listTime</c>"] List time
        /// </summary>
        [JsonPropertyName("listTime")]
        public DateTime ListTime { get; set; }
        /// <summary>
        /// ["<c>expireTime</c>"] Expire time
        /// </summary>
        [JsonPropertyName("expireTime")]
        public DateTime? ExpireTime { get; set; }
        /// <summary>
        /// ["<c>maxLeverage</c>"] Max leverage
        /// </summary>
        [JsonPropertyName("maxLeverage")]
        public decimal? MaxLeverage { get; set; }
        /// <summary>
        /// ["<c>minSize</c>"] Min order quantity in base asset
        /// </summary>
        [JsonPropertyName("minSize")]
        public decimal MinQuantity { get; set; }
        /// <summary>
        /// ["<c>lotSize</c>"] Quantity step in base asset
        /// </summary>
        [JsonPropertyName("lotSize")]
        public decimal LotSize { get; set; }
        /// <summary>
        /// ["<c>tickSize</c>"] Price step in quote asset
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal TickSize { get; set; }
        /// <summary>
        /// ["<c>instType</c>"] Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// ["<c>contractType</c>"] Contract type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType? ContractType { get; set; }
        /// <summary>
        /// ["<c>maxLimitSize</c>"] Max limit order quantity in base asset
        /// </summary>
        [JsonPropertyName("maxLimitSize")]
        public decimal MaxLimitQuantity { get; set; }
        /// <summary>
        /// ["<c>maxMarketSize</c>"] Max market order quantity in base asset
        /// </summary>
        [JsonPropertyName("maxMarketSize")]
        public decimal MaxMarketQuantity { get; set; }
        /// <summary>
        /// ["<c>state</c>"] Status
        /// </summary>
        [JsonPropertyName("state")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// ["<c>settleCurrency</c>"] Settle asset
        /// </summary>
        [JsonPropertyName("settleCurrency")]
        public string? SettleAsset { get; set; }
        /// <summary>
        /// ["<c>thresholdX</c>"] X threshold
        /// </summary>
        [JsonPropertyName("thresholdX")]
        public decimal? ThresholdX { get; set; }
        /// <summary>
        /// ["<c>thresholdY</c>"] Y threshold
        /// </summary>
        [JsonPropertyName("thresholdY")]
        public decimal? ThresholdY { get; set; }
        /// <summary>
        /// ["<c>thresholdZ</c>"] Z threshold
        /// </summary>
        [JsonPropertyName("thresholdZ")]
        public decimal? ThresholdZ { get; set; }
    }
}
