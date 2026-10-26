using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Spot websocket kline info. Spot streams omit the contract-volume field returned by REST.
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<BloFinSpotKline>))]
    public record BloFinSpotKline
    {
        /// <summary>
        /// [<c>0</c>] Open timestamp
        /// </summary>
        [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// [<c>1</c>] Open price
        /// </summary>
        [ArrayProperty(1)]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// [<c>2</c>] High price
        /// </summary>
        [ArrayProperty(2)]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// [<c>3</c>] Low price
        /// </summary>
        [ArrayProperty(3)]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// [<c>4</c>] Close price
        /// </summary>
        [ArrayProperty(4)]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// [<c>5</c>] Volume in base asset
        /// </summary>
        [ArrayProperty(5)]
        public decimal BaseVolume { get; set; }
        /// <summary>
        /// [<c>6</c>] Volume in quote asset
        /// </summary>
        [ArrayProperty(6)]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// [<c>7</c>] Whether the kline is finished
        /// </summary>
        [ArrayProperty(7), JsonConverter(typeof(BoolConverter))]
        public bool Finished { get; set; }
    }
}
