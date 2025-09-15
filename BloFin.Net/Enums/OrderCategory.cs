using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Order category
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderCategory>))]
    public enum OrderCategory
    {
        /// <summary>
        /// Normal
        /// </summary>
        [Map("normal")]
        Normal,
        /// <summary>
        /// Full liquidation
        /// </summary>
        [Map("full_liquidation")]
        FullLiquidation,
        /// <summary>
        /// Partial liquidation
        /// </summary>
        [Map("partial_liquidation")]
        PartialLiquidation,
        /// <summary>
        /// Auto deleveraging
        /// </summary>
        [Map("adl")]
        Adl,
        /// <summary>
        /// Take profit
        /// </summary>
        [Map("tp")]
        TakeProfit,
        /// <summary>
        /// Stop loss
        /// </summary>
        [Map("sl")]
        StopLoss,
        /// <summary>
        /// Pre TP/SL
        /// </summary>
        [Map("pre_tp_sl")]
        PreTpSl
    }
}
