using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderStatus>))]
    public enum OrderStatus
    {
        /// <summary>
        /// ["<c>live</c>"] Unfilled
        /// </summary>
        [Map("live")]
        Open,
        /// <summary>
        /// ["<c>partially_filled</c>"] Partially filled
        /// </summary>
        [Map("partially_filled")]
        PartiallyFilled,
        /// <summary>
        /// ["<c>canceled</c>"] Canceled
        /// </summary>
        [Map("canceled")]
        Canceled,
        /// <summary>
        /// ["<c>filled</c>"] Filled
        /// </summary>
        [Map("filled")]
        Filled,
        /// <summary>
        /// ["<c>partially_canceled</c>"] Partially filled / partially canceled
        /// </summary>
        [Map("partially_canceled")]
        PartiallyCanceled
    }
}
