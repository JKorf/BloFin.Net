using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Account level
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AccountLevel>))]
    public enum AccountLevel
    {
        /// <summary>
        /// Normal account
        /// </summary>
        [Map("0")]
        Normal,
        /// <summary>
        /// Unified spot account
        /// </summary>
        [Map("1")]
        UnifiedSpot,
        /// <summary>
        /// Unified spot futures account
        /// </summary>
        [Map("2")]
        UnifiedSpotFutures,
        /// <summary>
        /// Unified multi asset account
        /// </summary>
        [Map("3")]
        UnifiedMultiAsset
    }
}
