using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Asset type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AssetType>))]
    public enum AssetType
    {
        /// <summary>
        /// ["<c>base_currency</c>"] Base asset
        /// </summary>
        [Map("base_currency")]
        Base,
        /// <summary>
        /// ["<c>quote_currency</c>"] Quote asset
        /// </summary>
        [Map("quote_currency")]
        Quote,
    }
}
