using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Objects.Models;
using BloFin.Net.Objects.Options;
using BloFin.Net.Objects.Sockets.Subscriptions;
using System.Linq;
using CryptoExchange.Net;
using System.Collections.Generic;
using BloFin.Net.Enums;
using BloFin.Net.Objects.Sockets;

namespace BloFin.Net.Clients.FuturesApi
{
    /// <summary>
    /// Client providing access to the BloFin Futures websocket Api
    /// </summary>
    internal partial class BloFinSocketClientFuturesApi : SocketApiClient, IBloFinSocketClientFuturesApi
    {
        #region fields
        private static readonly MessagePath _eventPath = MessagePath.Get().Property("event");
        private static readonly MessagePath _channelPath = MessagePath.Get().Property("arg").Property("channel");
        private static readonly MessagePath _symbolPath = MessagePath.Get().Property("arg").Property("instId");

        protected override ErrorMapping ErrorMapping => BloFinErrors.Errors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal BloFinSocketClientFuturesApi(ILogger logger, BloFinSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.ExchangeOptions)
        {
            ProcessUnparsableMessages = true;
            RateLimiter = BloFinExchange.RateLimiter.BloFinSocket;

            RegisterPeriodicQuery(
                "Ping",
                TimeSpan.FromSeconds(30),
                x => new BloFinPingQuery(),
                (connection, result) =>
                {
                    if (result.Error?.ErrorType == ErrorType.Timeout)
                    {
                        // Ping timeout, reconnect
                        _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
                        _ = connection.TriggerReconnectAsync();
                    }
                });
        }
        #endregion

        /// <inheritdoc />
        protected override IByteMessageAccessor CreateAccessor(WebSocketMessageType type) => new SystemTextJsonByteMessageAccessor(BloFinExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(BloFinExchange._serializerContext);

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BloFinAuthenticationProvider(credentials);

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BloFinTrade[]>> onMessage, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BloFinTrade[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BloFinSubscription<BloFinTrade[]>(_logger, this, "trades", symbols.ToArray(), onMessage, false, false);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/public"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BloFinKline>> onMessage, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync([symbol], interval, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BloFinKline>> onMessage, CancellationToken ct = default)
        {
            var intervalStr = EnumConverter.GetString(interval);
            var subscription = new BloFinSubscription<BloFinKline[]>(_logger, this, "candle" + intervalStr, symbols.ToArray(), x => onMessage(x.As(x.Data.First())), false, false);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/public"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BloFinIndexMarkKline>> onMessage, CancellationToken ct = default)
            => SubscribeToIndexPriceKlineUpdatesAsync([symbol], interval, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BloFinIndexMarkKline>> onMessage, CancellationToken ct = default)
        {
            var intervalStr = EnumConverter.GetString(interval);
            var subscription = new BloFinSubscription<BloFinIndexMarkKline[]>(_logger, this, "index-candle" + intervalStr, symbols.ToArray(), x => onMessage(x.As(x.Data.First())), false, false);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/public"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BloFinIndexMarkKline>> onMessage, CancellationToken ct = default)
            => SubscribeToMarkPriceKlineUpdatesAsync([symbol], interval, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BloFinIndexMarkKline>> onMessage, CancellationToken ct = default)
        {
            var intervalStr = EnumConverter.GetString(interval);
            var subscription = new BloFinSubscription<BloFinIndexMarkKline[]>(_logger, this, "mark-price-candle" + intervalStr, symbols.ToArray(), x => onMessage(x.As(x.Data.First())), false, false);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/public"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<BloFinOrderBookUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync([symbol], depth, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<BloFinOrderBookUpdate>> onMessage, CancellationToken ct = default)
        {
            // Documentation states that the books subscription would publish 200 depth, but it's actually 400. Accept both
            depth.ValidateIntValues(nameof(depth), [5, 200, 400]);

            var subscription = new BloFinSubscription<BloFinOrderBookUpdate>(_logger, this, "books" + (depth == 5 ? "5" : ""), symbols.ToArray(), onMessage, false, false);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/public"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BloFinTicker>> onMessage, CancellationToken ct = default)
            => SubscribeToTickerUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BloFinTicker>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BloFinSubscription<BloFinTicker[]>(_logger, this, "tickers", symbols.ToArray(), x => onMessage(x.As(x.Data.First())), false, false);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/public"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToFundingRateUpdatesAsync(string symbol, Action<DataEvent<BloFinFundingRate>> onMessage, CancellationToken ct = default)
            => SubscribeToFundingRateUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToFundingRateUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BloFinFundingRate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BloFinSubscription<BloFinFundingRate[]>(_logger, this, "funding-rate", symbols.ToArray(), x => onMessage(x.As(x.Data.First())), false, false);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/public"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(Action<DataEvent<BloFinPosition[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BloFinSubscription<BloFinPosition[]>(_logger, this, "positions", null, onMessage, true, true);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/private"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BloFinOrder[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BloFinSubscription<BloFinOrder[]>(_logger, this, "orders", null, onMessage, true, true);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/private"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTriggerOrderUpdatesAsync(Action<DataEvent<BloFinAlgoOrderUpdate[]>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BloFinSubscription<BloFinAlgoOrderUpdate[]>(_logger, this, "orders-algo", null, onMessage, true, true);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/private"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BloFinFuturesBalances>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BloFinSubscription<BloFinFuturesBalances>(_logger, this, "account", null, onMessage, true, true);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/private"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToInverseBalanceUpdatesAsync(Action<DataEvent<BloFinFuturesInverseBalanceUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new BloFinSubscription<BloFinFuturesInverseBalanceUpdate>(_logger, this, "inverse-account", null, onMessage, true, true);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/private"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            if (!message.IsValid)
                return "pong";

            var evnt = message.GetValue<string?>(_eventPath);
            var channel = message.GetValue<string>(_channelPath);
            var symbol = message.GetValue<string>(_symbolPath);
            return evnt + channel + symbol;
        }

        /// <inheritdoc />
        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection)
        {
            var authParams = ((BloFinAuthenticationProvider)AuthenticationProvider!).GetSocketParameters();
            return Task.FromResult<Query?>(new BloFinLoginQuery(this, authParams, false));
        }

        /// <inheritdoc />
        public IBloFinSocketClientFuturesApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => BloFinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
