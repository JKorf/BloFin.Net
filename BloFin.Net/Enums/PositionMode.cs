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
        /// ["<c>net_mode</c>"] One way / net mode
        /// </summary>
        [Map("net_mode")]
        OneWayMode,
        /// <summary>
        /// ["<c>long_short_mode</c>"] Hedge / long-short mode
        /// </summary>
        [Map("long_short_mode")]
        HedgeMode,
    }
}
