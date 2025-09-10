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
        /// Linear contract
        /// </summary>
        [Map("linear")]
        Linear,
        /// <summary>
        /// Inverse contract
        /// </summary>
        [Map("inverse")]
        Inverse
    }
}
