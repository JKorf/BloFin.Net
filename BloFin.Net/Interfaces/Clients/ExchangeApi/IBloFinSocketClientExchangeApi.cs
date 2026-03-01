using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using BloFin.Net.Objects.Models;
using System.Collections.Generic;
using BloFin.Net.Enums;
using CryptoExchange.Net.Interfaces.Clients;

namespace BloFin.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// BloFin Exchange streams
    /// </summary>
    public interface IBloFinSocketClientFuturesApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Subscribe to live trade updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-trades-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: trades)
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BloFinTrade[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live trade updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-trades-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: trades)
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BloFinTrade[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick data update
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-candlesticks-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: candle{interval})
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BloFinKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick data update
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-candlesticks-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: candle{interval})
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BloFinKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick index price data update
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-index-candlesticks-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: index-candle{interval})
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BloFinIndexMarkKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick index price data update
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-index-candlesticks-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: index-candle{interval})
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BloFinIndexMarkKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick mark price data update
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-mark-price-candlesticks-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: mark-price-candle{interval})
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BloFinIndexMarkKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick mark price data update
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-mark-price-candlesticks-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: mark-price-candle{interval})
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BloFinIndexMarkKline>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-order-book-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: books{depth})
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="depth">The depth of the book, 5 or 400</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<BloFinOrderBookUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-order-book-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: books{depth})
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="depth">The depth of the book, 5 or 400. When using a depth 5 the full book snapshot will be pushed with each update</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<BloFinOrderBookUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to price ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-tickers-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: tickers)
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BloFinTicker>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to price ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-tickers-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: tickers)
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BloFinTicker>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to funding rate updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-funding-rate-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: funding-rate)
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToFundingRateUpdatesAsync(string symbol, Action<DataEvent<BloFinFundingRate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to funding rate updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-funding-rate-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/public (channel: funding-rate)
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToFundingRateUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BloFinFundingRate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to position updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-positions-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/private (channel: positions)
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(Action<DataEvent<BloFinPosition[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-order-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/private (channel: orders)
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BloFinOrder[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trigger order updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-algo-orders-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/private (channel: orders-algo)
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTriggerOrderUpdatesAsync(Action<DataEvent<BloFinAlgoOrderUpdate[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to futures balances updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-account-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/private (channel: account)
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BloFinFuturesBalances>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to inverse futures balances updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#ws-inverse-account-channel" /><br />
        /// Endpoint:<br />
        /// WS /ws/private (channel: inverse-account)
        /// </para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToInverseBalanceUpdatesAsync(Action<DataEvent<BloFinFuturesInverseBalanceUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IBloFinSocketClientFuturesApiShared SharedClient { get; }
    }
}
