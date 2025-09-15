using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Withdrawal status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<WithdrawalStatus>))]
    public enum WithdrawalStatus
    {
        /// <summary>
        /// Waiting for review
        /// </summary>
        [Map("0")]
        WaitingReview,
        /// <summary>
        /// Failed
        /// </summary>
        [Map("2")]
        Failed,
        /// <summary>
        /// Success
        /// </summary>
        [Map("3")]
        Success,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("4")]
        Canceled,
        /// <summary>
        /// Kyt
        /// </summary>
        [Map("6")]
        Kyt,
        /// <summary>
        /// Processing
        /// </summary>
        [Map("7")]
        Processing
    }
}
