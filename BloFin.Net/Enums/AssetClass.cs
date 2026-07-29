using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Asset class
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AssetClass>))]
    public enum AssetClass
    {
        /// <summary>
        /// ["<c>Crypto</c>"] Crypto
        /// </summary>
        [Map("Crypto")]
        Crypto,
        /// <summary>
        /// ["<c>Commodities</c>"] Commodities
        /// </summary>
        [Map("Commodities")]
        Commodities,
        /// <summary>
        /// ["<c>Stocks</c>"] Stocks
        /// </summary>
        [Map("Stocks")]
        Stocks,
        /// <summary>
        /// ["<c>Indices</c>"] Indices
        /// </summary>
        [Map("Indices")]
        Indices
    }
}
