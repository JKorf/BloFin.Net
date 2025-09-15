using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderType>))]
    public enum OrderType
    {
        /// <summary>
        /// Market
        /// </summary>
        [Map("market")]
        Market,
        /// <summary>
        /// Limit
        /// </summary>
        [Map("limit")]
        Limit,
        /// <summary>
        /// Post only
        /// </summary>
        [Map("post_only")]
        PostOnly,
        /// <summary>
        /// Fill or kill
        /// </summary>
        [Map("fok")]
        FillOrKill,
        /// <summary>
        /// Immediate or cancel
        /// </summary>
        [Map("ioc")]
        ImmediateOrCancel
    }
}
