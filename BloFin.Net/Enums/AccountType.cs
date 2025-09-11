using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Account type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AccountType>))]
    public enum AccountType
    {
        /// <summary>
        /// Funding account
        /// </summary>
        [Map("funding")]
        Funding,
        /// <summary>
        /// Futures account
        /// </summary>
        [Map("futures")]
        Futures,
        /// <summary>
        /// Copy trading account
        /// </summary>
        [Map("copy_trading")]
        CopyTrading,
        /// <summary>
        /// Earn account
        /// </summary>
        [Map("earn")]
        Earn,
        /// <summary>
        /// Spot account
        /// </summary>
        [Map("spot")]
        Spot,
        /// <summary>
        /// Inverse contract account
        /// </summary>
        [Map("inverse_contract")]
        InverseContract
    }
}
