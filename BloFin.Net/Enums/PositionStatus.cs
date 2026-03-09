using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Enums
{
    /// <summary>
    /// Position status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PositionStatus>))]
    public enum PositionStatus
    {
        /// <summary>
        /// ["<c>partially_closed</c>"] Partially closed
        /// </summary>
        [Map("partially_closed")]
        PartiallyClosed,
        /// <summary>
        /// ["<c>closed</c>"] Fully closed
        /// </summary>
        [Map("closed")]
        Closed,
        /// <summary>
        /// ["<c>liquidated</c>"] Liquidated
        /// </summary>
        [Map("liquidated")]
        Liquidated,
        /// <summary>
        /// ["<c>partially_liquidated</c>"] Partially liquidated
        /// </summary>
        [Map("partially_liquidated")]
        PartiallyLiquidated,
        /// <summary>
        /// ["<c>adl</c>"] Auto deleverage
        /// </summary>
        [Map("adl")]
        Adl,
    }

}
