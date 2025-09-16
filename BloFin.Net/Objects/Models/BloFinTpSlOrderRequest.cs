using BloFin.Net.Enums;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Tp/Sl order request
    /// </summary>
    public record BloFinTpSlOrderRequest
    {
        /// <summary>
        /// Take profit trigger price
        /// </summary>
        [JsonPropertyName("tpTriggerPrice"), JsonConverter(typeof(DecimalStringWriterConverter)), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? TakeProfitTriggerPrice { get; set; }
        /// <summary>
        /// Take profit order price
        /// </summary>
        [JsonPropertyName("tpOrderPrice"), JsonConverter(typeof(DecimalStringWriterConverter)), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? TakeProfitOrderPrice { get; set; }
        /// <summary>
        /// Take profit trigger price
        /// </summary>
        [JsonPropertyName("tpTriggerPriceType"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TriggerPriceType? TakeProfitTriggerPriceType { get; set; }
        /// <summary>
        /// Stop loss trigger price
        /// </summary>
        [JsonPropertyName("slTriggerPrice"), JsonConverter(typeof(DecimalStringWriterConverter)), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? StopLossTriggerPrice { get; set; }
        /// <summary>
        /// Stop loss order price
        /// </summary>
        [JsonPropertyName("slOrderPrice"), JsonConverter(typeof(DecimalStringWriterConverter)), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? StopLossOrderPrice { get; set; }
        /// <summary>
        /// Stop loss trigger price
        /// </summary>
        [JsonPropertyName("slTriggerPriceType"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TriggerPriceType? StopLossTriggerPriceType { get; set; }
    }
}
