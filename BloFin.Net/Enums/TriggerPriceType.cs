using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Trigger price type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TriggerPriceType>))]
    public enum TriggerPriceType
    {
        /// <summary>
        /// ["<c>last</c>"] Last price
        /// </summary>
        [Map("last")]
        LastPrice,
        /// <summary>
        /// ["<c>index</c>"] Index price
        /// </summary>
        [Map("index")]
        IndexPrice,
        /// <summary>
        /// ["<c>mark</c>"] Mark price
        /// </summary>
        [Map("mark", "market")]
        MarkPrice
    }
}
