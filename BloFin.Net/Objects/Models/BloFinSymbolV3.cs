using System;
using System.Text.Json.Serialization;
using BloFin.Net.Enums;

namespace BloFin.Net.Objects.Models;

/// <summary>
/// Symbol info
/// </summary>
public record BloFinSymbolV3
{
    /// <summary>
    /// ["<c>symbol</c>"] Symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>base_currency</c>"] Base asset
    /// </summary>
    [JsonPropertyName("base_currency")]
    public string BaseAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>quote_currency</c>"] Quote asset
    /// </summary>
    [JsonPropertyName("quote_currency")]
    public string QuoteAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>logo</c>"] Logo
    /// </summary>
    [JsonPropertyName("logo")]
    public string Logo { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>max_limit_order_quantity</c>"] Max limit order quantity
    /// </summary>
    [JsonPropertyName("max_limit_order_quantity")]
    public decimal MaxLimitOrderQuantity { get; set; }
    /// <summary>
    /// ["<c>max_market_order_quantity</c>"] Max market order quantity
    /// </summary>
    [JsonPropertyName("max_market_order_quantity")]
    public decimal MaxMarketOrderQuantity { get; set; }
    /// <summary>
    /// ["<c>contract_value</c>"] Contract value
    /// </summary>
    [JsonPropertyName("contract_value")]
    public decimal ContractValue { get; set; }
    /// <summary>
    /// ["<c>price_precision</c>"] Price precision
    /// </summary>
    [JsonPropertyName("price_precision")]
    public decimal PricePrecision { get; set; }
    /// <summary>
    /// ["<c>tick_size</c>"] Tick quantity
    /// </summary>
    [JsonPropertyName("tick_size")]
    public decimal TickQuantity { get; set; }
    /// <summary>
    /// ["<c>depth_step</c>"] Depth step
    /// </summary>
    [JsonPropertyName("depth_step")]
    public int DepthStep { get; set; }
    /// <summary>
    /// ["<c>tag_ids</c>"] Tag ids
    /// </summary>
    [JsonPropertyName("tag_ids")]
    public int[] TagIds { get; set; } = [];
    /// <summary>
    /// ["<c>trading_start_time</c>"] Trading start time
    /// </summary>
    [JsonPropertyName("trading_start_time")]
    public DateTime? TradingStartTime { get; set; }
    /// <summary>
    /// ["<c>trading_end_time</c>"] Trading end time
    /// </summary>
    [JsonPropertyName("trading_end_time")]
    public DateTime? TradingEndTime { get; set; }
    /// <summary>
    /// ["<c>sort</c>"] Sort
    /// </summary>
    [JsonPropertyName("sort")]
    public long Sort { get; set; }
    /// <summary>
    /// ["<c>reduce_only_time</c>"] Reduce only time
    /// </summary>
    [JsonPropertyName("reduce_only_time")]
    public DateTime? ReduceOnlyTime { get; set; }
    /// <summary>
    /// ["<c>delisting_time</c>"] Delisting time
    /// </summary>
    [JsonPropertyName("delisting_time")]
    public DateTime? DelistingTime { get; set; }
    /// <summary>
    /// ["<c>contract_value_multiplier</c>"] Contract value multiplier
    /// </summary>
    [JsonPropertyName("contract_value_multiplier")]
    public decimal ContractValueMultiplier { get; set; }
    /// <summary>
    /// ["<c>contract_type</c>"] Contract type
    /// </summary>
    [JsonPropertyName("contract_type")]
    public ContractType ContractType { get; set; }
    /// <summary>
    /// ["<c>settle_currency</c>"] Settle asset
    /// </summary>
    [JsonPropertyName("settle_currency")]
    public string SettleAsset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>currency_type</c>"] Asset type
    /// </summary>
    [JsonPropertyName("currency_type")]
    public string AssetType { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>lot_size</c>"] Lot quantity
    /// </summary>
    [JsonPropertyName("lot_size")]
    public decimal LotQuantity { get; set; }
    /// <summary>
    /// ["<c>threshold_x</c>"] Threshold x
    /// </summary>
    [JsonPropertyName("threshold_x")]
    public decimal ThresholdX { get; set; }
    /// <summary>
    /// ["<c>threshold_y</c>"] Threshold y
    /// </summary>
    [JsonPropertyName("threshold_y")]
    public decimal ThresholdY { get; set; }
    /// <summary>
    /// ["<c>threshold_z</c>"] Threshold z
    /// </summary>
    [JsonPropertyName("threshold_z")]
    public decimal ThresholdZ { get; set; }
    /// <summary>
    /// ["<c>is_innovation</c>"] Is innovation
    /// </summary>
    [JsonPropertyName("is_innovation")]
    public bool IsInnovation { get; set; }
    /// <summary>
    /// ["<c>is_xstocks</c>"] Is xstocks
    /// </summary>
    [JsonPropertyName("is_xstocks")]
    public bool IsXstocks { get; set; }
    /// <summary>
    /// ["<c>is_pre_ipo</c>"] Is pre ipo
    /// </summary>
    [JsonPropertyName("is_pre_ipo")]
    public bool IsPreIpo { get; set; }
    /// <summary>
    /// ["<c>isolated_only</c>"] Isolated only
    /// </summary>
    [JsonPropertyName("isolated_only")]
    public bool IsolatedOnly { get; set; }
    /// <summary>
    /// ["<c>halt_start_time</c>"] Halt start time
    /// </summary>
    [JsonPropertyName("halt_start_time")]
    public DateTime? HaltStartTime { get; set; }
    /// <summary>
    /// ["<c>halt_end_time</c>"] Halt end time
    /// </summary>
    [JsonPropertyName("halt_end_time")]
    public DateTime? HaltEndTime { get; set; }
}

