using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Product type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ProductType>))]
    public enum ProductType
    {
        /// <summary>
        /// USDT futures
        /// </summary>
        [Map("USDT-FUTURES")]
        UsdtFutures,
        /// <summary>
        /// Coin futures
        /// </summary>
        [Map("COIN-FUTURES")]
        CoinFutures
    }
}
