using BloFin.Net.Enums;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BloFin Spot streams
    /// </summary>
    public interface IBloFinSocketClientSpotApi : ISocketApiClient<BloFinCredentials>, IDisposable
    {
        /// <summary>
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        public IBloFinSocketClientSpotSharedApi SharedApi { get; }

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

        /// <summary>
        /// Subscribe to order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-books-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/public (channel: books)
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="depth">The depth of the order book, 5 or 400. 400 first sends a snapshot with incremental changes after</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<BloFinOrderBookUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-books-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/public (channel: books)
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="depth">The depth of the order book, 5 or 400. 400 first sends a snapshot with incremental changes after</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<BloFinOrderBookUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-order-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/private (channel: orders)
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BloFinOrder[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to algo order updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-algo-orders-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/private (channel: orders-algo)
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToAlgoOrderUpdatesAsync(Action<DataEvent<BloFinAlgoOrderUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to balances updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#ws-trading-account-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/spot/private (channel: account)
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BloFinFuturesBalances>> onMessage, CancellationToken ct = default);
    }
}
