using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Funding kind
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FundingKind>))]
    public enum FundingKind
    {
        /// <summary>
        /// ["<c>0</c>"] Regular funding
        /// </summary>
        [Map("0")]
        Regular,
        /// <summary>
        /// ["<c>1</c>"] Stock dividend
        /// </summary>
        [Map("1")]
        StockDividend,
    }
}
