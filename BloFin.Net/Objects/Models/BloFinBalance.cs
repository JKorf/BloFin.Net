using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    public record BloFinBalance
    {
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>balance</c>"] Total balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal TotalBalance { get; set; }
        /// <summary>
        /// ["<c>available</c>"] Available balance
        /// </summary>
        [JsonPropertyName("available")]
        public decimal AvailableBalance { get; set; }
        /// <summary>
        /// ["<c>frozen</c>"] Frozen balance
        /// </summary>
        [JsonPropertyName("frozen")]
        public decimal FrozenBalance { get; set; }
        /// <summary>
        /// ["<c>bonus</c>"] Bonus
        /// </summary>
        [JsonPropertyName("bonus")]
        public decimal Bonus { get; set; }
    }
}
