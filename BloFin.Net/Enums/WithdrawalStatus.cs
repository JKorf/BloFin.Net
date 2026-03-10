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
        /// ["<c>0</c>"] Waiting for review
        /// </summary>
        [Map("0")]
        WaitingReview,
        /// <summary>
        /// ["<c>2</c>"] Failed
        /// </summary>
        [Map("2")]
        Failed,
        /// <summary>
        /// ["<c>3</c>"] Success
        /// </summary>
        [Map("3")]
        Success,
        /// <summary>
        /// ["<c>4</c>"] Canceled
        /// </summary>
        [Map("4")]
        Canceled,
        /// <summary>
        /// ["<c>6</c>"] Kyt
        /// </summary>
        [Map("6")]
        Kyt,
        /// <summary>
        /// ["<c>7</c>"] Processing
        /// </summary>
        [Map("7")]
        Processing
    }
}
