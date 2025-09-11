using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Api key info
    /// </summary>
    public record BloFinApiKey
    {
        /// <summary>
        /// User UID
        /// </summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// API key name
        /// </summary>
        [JsonPropertyName("apiName")]
        public string ApiKeyName { get; set; } = string.Empty;
        /// <summary>
        /// API key
        /// </summary>
        [JsonPropertyName("apiKey")]
        public string ApiKey { get; set; } = string.Empty;
        /// <summary>
        /// Is readonly
        /// </summary>
        [JsonPropertyName("readOnly")]
        public bool IsReadOnly { get; set; }
        /// <summary>
        /// Bound IP addresses
        /// </summary>
        [JsonPropertyName("ips")]
        public string[] Ips { get; set; } = [];
        /// <summary>
        /// Key type
        /// </summary>
        [JsonPropertyName("type")]
        public int KeyType { get; set; }
        /// <summary>
        /// Key expire time
        /// </summary>
        [JsonPropertyName("expireTime")]
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Referral code
        /// </summary>
        [JsonPropertyName("referralCode")]
        public string ReferralCode { get; set; } = string.Empty;
        /// <summary>
        /// Parent account UID
        /// </summary>
        [JsonPropertyName("parentUid")]
        public string ParentUid { get; set; } = string.Empty;
    }
}
