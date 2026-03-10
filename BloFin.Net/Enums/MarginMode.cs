using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Margin mode
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MarginMode>))]
    public enum MarginMode
    {
        /// <summary>
        /// ["<c>isolated</c>"] Isolated
        /// </summary>
        [Map("isolated")]
        Isolated,
        /// <summary>
        /// ["<c>cross</c>"] Cross
        /// </summary>
        [Map("cross")]
        Cross
    }
}
