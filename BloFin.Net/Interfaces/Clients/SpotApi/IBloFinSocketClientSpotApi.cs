using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using BloFin.Net.Objects.Models;
using System.Collections.Generic;
using BloFin.Net.Enums;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Authentication;

namespace BloFin.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BloFin Spot streams
    /// </summary>
    public interface IBloFinSocketClientSpotApi : ISocketApiClient<BloFinCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to live trade updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-trades-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/public (channel: trades)
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BloFinTrade[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live trade updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-trades-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/public (channel: trades)
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BloFinTrade[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick data update
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-candlesticks-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/public (channel: candle{interval})
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BloFinSpotKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick data update
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-candlesticks-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/public (channel: candle{interval})
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BloFinSpotKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to price ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-tickers-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/public (channel: tickers)
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BloFinSpotTicker>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to price ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-tickers-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/public (channel: tickers)
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BloFinSpotTicker>> onMessage, CancellationToken ct = default);
    }
}
