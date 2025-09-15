using BloFin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Transfer info
    /// </summary>
    public record BloFinTransfer
    {
        /// <summary>
        /// Transfer id
        /// </summary>
        [JsonPropertyName("transferId")]
        public string TransferId { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// From account
        /// </summary>
        [JsonPropertyName("fromAccount")]
        public AccountType FromAccount { get; set; }
        /// <summary>
        /// To account
        /// </summary>
        [JsonPropertyName("toAccount")]
        public AccountType ToAccount { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Client id
        /// </summary>
        [JsonPropertyName("clientId")]
        public string? ClientId { get; set; }
    }


}
