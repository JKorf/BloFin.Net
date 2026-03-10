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
        /// ["<c>market</c>"] Market
        /// </summary>
        [Map("market")]
        Market,
        /// <summary>
        /// ["<c>limit</c>"] Limit
        /// </summary>
        [Map("limit")]
        Limit,
        /// <summary>
        /// ["<c>post_only</c>"] Post only
        /// </summary>
        [Map("post_only")]
        PostOnly,
        /// <summary>
        /// ["<c>fok</c>"] Fill or kill
        /// </summary>
        [Map("fok")]
        FillOrKill,
        /// <summary>
        /// ["<c>ioc</c>"] Immediate or cancel
        /// </summary>
        [Map("ioc")]
        ImmediateOrCancel
    }
}
