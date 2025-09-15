using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Position mode
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PositionMode>))]
    public enum PositionMode
    {
        /// <summary>
        /// One way / net mode
        /// </summary>
        [Map("net_mode")]
        OneWayMode,
        /// <summary>
        /// Hedge / long-short mode
        /// </summary>
        [Map("long_short_mode")]
        HedgeMode,
    }
}
