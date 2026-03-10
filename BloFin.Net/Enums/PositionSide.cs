using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Position side
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PositionSide>))]
    public enum PositionSide
    {
        /// <summary>
        /// ["<c>long</c>"] Long position
        /// </summary>
        [Map("long")]
        Long,
        /// <summary>
        /// ["<c>short</c>"] Short position
        /// </summary>
        [Map("short")]
        Short,
        /// <summary>
        /// ["<c>net</c>"] Net position
        /// </summary>
        [Map("net")]
        Net,
    }
}
