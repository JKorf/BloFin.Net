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
        /// ["<c>funding</c>"] Funding account
        /// </summary>
        [Map("funding")]
        Funding,
        /// <summary>
        /// ["<c>futures</c>"] Futures account
        /// </summary>
        [Map("futures")]
        Futures,
        /// <summary>
        /// ["<c>copy_trading</c>"] Copy trading account
        /// </summary>
        [Map("copy_trading")]
        CopyTrading,
        /// <summary>
        /// ["<c>earn</c>"] Earn account
        /// </summary>
        [Map("earn")]
        Earn,
        /// <summary>
        /// ["<c>spot</c>"] Spot account
        /// </summary>
        [Map("spot")]
        Spot,
        /// <summary>
        /// ["<c>inverse_contract</c>"] Inverse contract account
        /// </summary>
        [Map("inverse_contract")]
        InverseContract
    }
}
