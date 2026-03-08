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
        /// ["<c>asks</c>"] Asks
        /// </summary>
        [JsonPropertyName("asks")]
        public BloFinOrderBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// ["<c>bids</c>"] Bids
        /// </summary>
        [JsonPropertyName("bids")]
        public BloFinOrderBookEntry[] Bids { get; set; } = [];
        /// <summary>
        /// ["<c>ts</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>prevSeqId</c>"] Previous sequence id
        /// </summary>
        [JsonPropertyName("prevSeqId")]
        public long? PrevSequence { get; set; }
        /// <summary>
        /// ["<c>seqId</c>"] Sequence id
        /// </summary>
        [JsonPropertyName("seqId")]
        public long? Sequence { get; set; }
    }
}
