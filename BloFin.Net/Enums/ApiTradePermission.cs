using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// API trading permissions
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ApiTradePermission>))]
    public enum ApiTradePermission
    {
        /// <summary>
        /// ["<c>full</c>"] Full
        /// </summary>
        [Map("full")]
        Full,
        /// <summary>
        /// ["<c>reduce_only</c>"] Reduce only
        /// </summary>
        [Map("reduce_only")]
        ReduceOnly
    }
}
