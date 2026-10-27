using BloFin.Net.Enums;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BloFin public spot market data
    /// </summary>
    public interface IBloFinRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Get spot instruments and their trading constraints.
        /// <para><a href="https://docs.blofin.com/spot.html#get-instruments" /></para>
        /// </summary>
        /// <param name="symbol">Optional symbol filter, for example BTC-USDT</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinSpotSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get spot prices and 24-hour base-asset volume.
        /// <para><a href="https://docs.blofin.com/spot.html#get-tickers" /></para>
        /// </summary>
        /// <param name="symbol">Optional symbol filter, for example BTC-USDT</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinSpotTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get order book snapshot
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-order-book" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/spot/market/books<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="depth">["<c>size</c>"] Book depth, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinOrderBook>> GetOrderBookAsync(
            string symbol,
            int depth,
            CancellationToken ct = default);

        /// <summary>
        /// 
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-trades" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/spot/market/trades<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinSpotTrade[]>> GetRecentTradesAsync(
            string symbol,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get candlestick data
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-candlesticks" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/spot/market/candles<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">["<c>bar</c>"] Kline interval</param>
        /// <param name="startTime">["<c>after</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>before</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 1440</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

    }
}
