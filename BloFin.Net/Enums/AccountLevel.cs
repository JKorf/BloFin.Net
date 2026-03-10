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
        /// ["<c>0</c>"] Normal account
        /// </summary>
        [Map("0")]
        Normal,
        /// <summary>
        /// ["<c>1</c>"] Unified spot account
        /// </summary>
        [Map("1")]
        UnifiedSpot,
        /// <summary>
        /// ["<c>2</c>"] Unified spot futures account
        /// </summary>
        [Map("2")]
        UnifiedSpotFutures,
        /// <summary>
        /// ["<c>3</c>"] Unified multi asset account
        /// </summary>
        [Map("3")]
        UnifiedMultiAsset
    }
}
