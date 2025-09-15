using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Deposit status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<DepositStatus>))]
    public enum DepositStatus
    {
        /// <summary>
        /// Pending
        /// </summary>
        [Map("0")]
        Pending,
        /// <summary>
        /// Done
        /// </summary>
        [Map("1")]
        Done,
        /// <summary>
        /// Failed
        /// </summary>
        [Map("2")]
        Failed,
        /// <summary>
        /// Kyt
        /// </summary>
        [Map("3")]
        Kyt
    }
}
