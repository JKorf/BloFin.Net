using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// TP/SL Order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TpSlOrderStatus>))]
    public enum TpSlOrderStatus
    {
        /// <summary>
        /// ["<c>live</c>"] Live
        /// </summary>
        [Map("live")]
        Live,
        /// <summary>
        /// ["<c>effective</c>"] Effective
        /// </summary>
        [Map("effective")]
        Effective,
        /// <summary>
        /// ["<c>canceled</c>"] Canceled
        /// </summary>
        [Map("canceled")]
        Canceled,
        /// <summary>
        /// ["<c>order_failed</c>"] Failed to place order
        /// </summary>
        [Map("order_failed")]
        Failed
    }
}
