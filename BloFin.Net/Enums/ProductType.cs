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
        /// ["<c>USDT-FUTURES</c>"] USDT futures
        /// </summary>
        [Map("USDT-FUTURES")]
        UsdtFutures,
        /// <summary>
        /// ["<c>COIN-FUTURES</c>"] Coin futures
        /// </summary>
        [Map("COIN-FUTURES")]
        CoinFutures
    }
}
