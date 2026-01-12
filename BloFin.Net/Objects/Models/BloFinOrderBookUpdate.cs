using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Order book update
    /// </summary>
    public record BloFinOrderBookUpdate
    {
        /// <summary>
        /// Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public BloFinOrderBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public BloFinOrderBookEntry[] Bids { get; set; } = [];
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Previous sequence id
        /// </summary>
        [JsonPropertyName("prevSeqId")]
        public long? PrevSequence { get; set; }
        /// <summary>
        /// Sequence id
        /// </summary>
        [JsonPropertyName("seqId")]
        public long? Sequence { get; set; }
    }
}
