using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Api key info
    /// </summary>
    public record BloFinApiKey
    {
        /// <summary>
        /// ["<c>uid</c>"] User UID
        /// </summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>apiName</c>"] API key name
        /// </summary>
        [JsonPropertyName("apiName")]
        public string ApiKeyName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>apiKey</c>"] API key
        /// </summary>
        [JsonPropertyName("apiKey")]
        public string ApiKey { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>readOnly</c>"] Is readonly
        /// </summary>
        [JsonPropertyName("readOnly")]
        public bool IsReadOnly { get; set; }
        /// <summary>
        /// ["<c>ips</c>"] Bound IP addresses
        /// </summary>
        [JsonPropertyName("ips")]
        public string[] Ips { get; set; } = [];
        /// <summary>
        /// ["<c>type</c>"] Key type
        /// </summary>
        [JsonPropertyName("type")]
        public int KeyType { get; set; }
        /// <summary>
        /// ["<c>expireTime</c>"] Key expire time
        /// </summary>
        [JsonPropertyName("expireTime")]
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// ["<c>createTime</c>"] Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>referralCode</c>"] Referral code
        /// </summary>
        [JsonPropertyName("referralCode")]
        public string ReferralCode { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>parentUid</c>"] Parent account UID
        /// </summary>
        [JsonPropertyName("parentUid")]
        public string ParentUid { get; set; } = string.Empty;
    }
}
