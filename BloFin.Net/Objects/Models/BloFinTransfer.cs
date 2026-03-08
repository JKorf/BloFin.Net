using BloFin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Transfer info
    /// </summary>
    public record BloFinTransfer
    {
        /// <summary>
        /// ["<c>transferId</c>"] Transfer id
        /// </summary>
        [JsonPropertyName("transferId")]
        public string TransferId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>fromAccount</c>"] From account
        /// </summary>
        [JsonPropertyName("fromAccount")]
        public AccountType FromAccount { get; set; }
        /// <summary>
        /// ["<c>toAccount</c>"] To account
        /// </summary>
        [JsonPropertyName("toAccount")]
        public AccountType ToAccount { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>ts</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>clientId</c>"] Client id
        /// </summary>
        [JsonPropertyName("clientId")]
        public string? ClientId { get; set; }
    }


}
