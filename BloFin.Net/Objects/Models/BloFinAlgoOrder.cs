using BloFin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Algo order info
    /// </summary>
    public record BloFinAlgoOrder
    {
        /// <summary>
        /// Algo order id
        /// </summary>
        [JsonPropertyName("algoId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Margin mode
        /// </summary>
        [JsonPropertyName("marginMode")]
        public MarginMode MarginMode { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("state")]
        public TpSlOrderStatus Status { get; set; }
        /// <summary>
        /// Actual quantity
        /// </summary>
        [JsonPropertyName("actualSize")]
        public decimal? ActualQuantity { get; set; }
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// Trigger price type
        /// </summary>
        [JsonPropertyName("triggerPriceType")]
        public TriggerPriceType? TriggerPriceType { get; set; }
        /// <summary>
        /// Broker id
        /// </summary>
        [JsonPropertyName("brokerId")]
        public string? BrokerId { get; set; }
        /// <summary>
        /// Attach algo orders
        /// </summary>
        [JsonPropertyName("attachAlgoOrders")]
        public BloFinAlgoAttachedOrder[] AttachAlgoOrders { get; set; } = [];
    }

    /// <summary>
    /// Attached order info
    /// </summary>
    public record BloFinAlgoAttachedOrder
    {
        /// <summary>
        /// Take profit trigger price
        /// </summary>
        [JsonPropertyName("tpTriggerPrice")]
        public decimal? TakeProfitTriggerPrice { get; set; }
        /// <summary>
        /// Take profit order price
        /// </summary>
        [JsonPropertyName("tpOrderPrice")]
        public decimal? TakeProfitOrderPrice { get; set; }
        /// <summary>
        /// Take profit trigger price type
        /// </summary>
        [JsonPropertyName("tpTriggerPriceType")]
        public TriggerPriceType? TakeProfitTriggerPriceType { get; set; }
        /// <summary>
        /// Stop loss trigger price
        /// </summary>
        [JsonPropertyName("slTriggerPrice")]
        public decimal? StopLossTriggerPrice { get; set; }
        /// <summary>
        /// Stop loss order price
        /// </summary>
        [JsonPropertyName("slOrderPrice")]
        public decimal? StopLossOrderPrice { get; set; }
        /// <summary>
        /// Stop loss trigger price type
        /// </summary>
        [JsonPropertyName("slTriggerPriceType")]
        public TriggerPriceType? StopLossTriggerPriceType { get; set; }
    } 


}
