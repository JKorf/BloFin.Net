using System;
using System.Threading;
using System.Threading.Tasks;
using BloFin.Net.Enums;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace BloFin.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// BloFin Exchange exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IBloFinRestClientExchangeApiExchangeData
    {
        /// <summary>
        /// Get list of supported symbols
        /// <para><a href="https://docs.blofin.com/index.html#get-instruments" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `BTC-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get price ticker info
        /// <para><a href="https://docs.blofin.com/index.html#get-tickers" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get the order book for a symbol
        /// <para><a href="https://docs.blofin.com/index.html#get-order-book" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="depth">The depth of the book, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Get recent trades
        /// <para><a href="https://docs.blofin.com/index.html#get-trades" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="limit">Number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get index and mark price for a symbol
        /// <para><a href="https://docs.blofin.com/index.html#get-mark-price" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinMarkIndexPrice>> GetIndexMarkPriceAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get funding rate info
        /// <para><a href="https://docs.blofin.com/index.html#get-funding-rate" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinFundingRate>> GetFundingRateAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get funding rate history
        /// <para><a href="https://docs.blofin.com/index.html#get-funding-rate-history" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinFundingRate[]>> GetFundingRateHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get Kline data for a symbol
        /// <para><a href="https://docs.blofin.com/index.html#get-candlesticks" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results, max 1440</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get index price Kline data for a symbol
        /// <para><a href="https://docs.blofin.com/index.html#get-index-candlesticks" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results, max 1440</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinIndexMarkKline[]>> GetIndexPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get mark price Kline data for a symbol
        /// <para><a href="https://docs.blofin.com/index.html#get-mark-price-candlesticks" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results, max 1440</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinIndexMarkKline[]>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);
    }
}
