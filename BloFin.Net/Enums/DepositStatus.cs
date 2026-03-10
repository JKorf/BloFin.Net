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
        /// ["<c>0</c>"] Pending
        /// </summary>
        [Map("0")]
        Pending,
        /// <summary>
        /// ["<c>1</c>"] Done
        /// </summary>
        [Map("1")]
        Done,
        /// <summary>
        /// ["<c>2</c>"] Failed
        /// </summary>
        [Map("2")]
        Failed,
        /// <summary>
        /// ["<c>3</c>"] Kyt
        /// </summary>
        [Map("3")]
        Kyt
    }
}
