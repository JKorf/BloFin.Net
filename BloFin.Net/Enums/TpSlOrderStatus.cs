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
        /// Live
        /// </summary>
        [Map("live")]
        Live,
        /// <summary>
        /// Effective
        /// </summary>
        [Map("effective")]
        Effective,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("canceled")]
        Canceled,
        /// <summary>
        /// Failed to place order
        /// </summary>
        [Map("order_failed")]
        Failed
    }
}
