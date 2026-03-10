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
        /// ["<c>normal</c>"] Normal
        /// </summary>
        [Map("normal")]
        Normal,
        /// <summary>
        /// ["<c>full_liquidation</c>"] Full liquidation
        /// </summary>
        [Map("full_liquidation")]
        FullLiquidation,
        /// <summary>
        /// ["<c>partial_liquidation</c>"] Partial liquidation
        /// </summary>
        [Map("partial_liquidation")]
        PartialLiquidation,
        /// <summary>
        /// ["<c>adl</c>"] Auto deleveraging
        /// </summary>
        [Map("adl")]
        Adl,
        /// <summary>
        /// ["<c>tp</c>"] Take profit
        /// </summary>
        [Map("tp")]
        TakeProfit,
        /// <summary>
        /// ["<c>sl</c>"] Stop loss
        /// </summary>
        [Map("sl")]
        StopLoss,
        /// <summary>
        /// ["<c>pre_tp_sl</c>"] Pre TP/SL
        /// </summary>
        [Map("pre_tp_sl")]
        PreTpSl
    }
}
