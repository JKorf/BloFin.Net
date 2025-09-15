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
        /// Last price
        /// </summary>
        [Map("last")]
        LastPrice,
        /// <summary>
        /// Index price
        /// </summary>
        [Map("index")]
        IndexPrice,
        /// <summary>
        /// Mark price
        /// </summary>
        [Map("mark", "market")]
        MarkPrice
    }
}
