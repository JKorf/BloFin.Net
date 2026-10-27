using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Funding rate info
    /// </summary>
    public record BloFinFundingRate
    {
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>fundingRate</c>"] Funding rate
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal FundingRate { get; set; }
        /// <summary>
        /// ["<c>fundingTime</c>"] Funding time
        /// </summary>
        [JsonPropertyName("fundingTime")]
        public DateTime FundingTime { get; set; }
        /// <summary>
        /// ["<c>fundingInterval</c>"] Funding interval
        /// </summary>
        [JsonPropertyName("fundingInterval")]
        public int FundingInterval { get; set; }
        /// <summary>
        /// ["<c>fundingRateCap</c>"] Funding rate cap
        /// </summary>
        [JsonPropertyName("fundingRateCap")]
        public decimal FundingRateCap { get; set; }
        /// <summary>
        /// ["<c>fundingRateFloor</c>"] Funding rate floor
        /// </summary>
        [JsonPropertyName("fundingRateFloor")]
        public decimal FundingRateFloor { get; set; }
        /// <summary>
        /// ["<c>fundingIntervalUnit</c>"] Funding interval unit
        /// </summary>
        [JsonPropertyName("fundingIntervalUnit")]
        public string FundingIntervalUnit { get; set; } = string.Empty;
    }


}
