using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Contract type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ContractType>))]
    public enum ContractType
    {
        /// <summary>
        /// ["<c>linear</c>"] Linear contract
        /// </summary>
        [Map("linear")]
        Linear,
        /// <summary>
        /// ["<c>inverse</c>"] Inverse contract
        /// </summary>
        [Map("inverse")]
        Inverse
    }
}
