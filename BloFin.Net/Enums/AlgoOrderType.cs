using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Algo order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AlgoOrderType>))]
    public enum AlgoOrderType
    {
        /// <summary>
        /// ["<c>conditional</c>"] Conditional
        /// </summary>
        [Map("conditional")]
        Conditional,
        /// <summary>
        /// ["<c>trigger</c>"] Trigger
        /// </summary>
        [Map("trigger")]
        Trigger
    }
}
