using System;
using System.Text.Json.Serialization;
using BloFin.Net.Enums;

namespace BloFin.Net.Objects.Models;

/// <summary>
/// Spot trade
/// </summary>
public record BloFinSpotTrade
{
    /// <summary>
    /// ["<c>tradeId</c>"] Trade id
    /// </summary>
    [JsonPropertyName("tradeId")]
    public string TradeId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>instId</c>"] Inst id
    /// </summary>
    [JsonPropertyName("instId")]
    public string InstId { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>price</c>"] Price
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    /// <summary>
    /// ["<c>size</c>"] Quantity
    /// </summary>
    [JsonPropertyName("size")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// ["<c>side</c>"] Side
    /// </summary>
    [JsonPropertyName("side")]
    public OrderSide Side { get; set; }
    /// <summary>
    /// ["<c>ts</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("ts")]
    public DateTime Timestamp { get; set; }
}

