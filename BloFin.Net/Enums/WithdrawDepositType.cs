using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Withdrawal/deposit type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<WithdrawDepositType>))]
    public enum WithdrawDepositType
    {
        /// <summary>
        /// Blockchain
        /// </summary>
        [Map("0")]
        Blockchain,
        /// <summary>
        /// Internal
        /// </summary>
        [Map("1")]
        Internal
    }
}
