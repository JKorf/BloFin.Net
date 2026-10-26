using BloFin.Net.Clients.MessageHandlers;
using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.SpotApi;
using BloFin.Net.Objects.Internal;
using BloFin.Net.Objects.Models;
using BloFin.Net.Objects.Options;
using BloFin.Net.Objects.Sockets;
using BloFin.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IBloFinSocketClientSpotApi" />
    internal class BloFinSocketClientSpotApi : SocketApiClient<BloFinEnvironment, BloFinAuthenticationProvider, BloFinCredentials>, IBloFinSocketClientSpotApi
    {
        /// <inheritdoc />
        protected override ErrorMapping ErrorMapping => BloFinErrors.Errors;

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(BloFinExchange._serializerContext);

        /// <inheritdoc />
        protected override BloFinAuthenticationProvider CreateAuthenticationProvider(BloFinCredentials credentials) => new BloFinAuthenticationProvider(credentials);

        /// <summary>
        /// Create the public spot websocket client
        /// </summary>
        internal BloFinSocketClientSpotApi(ILoggerFactory? loggerFactory, BloFinSocketOptions options)
            : base(loggerFactory, BloFinExchange.Metadata.Id, options.Environment.SocketClientAddress, options, options.ExchangeOptions)
        {
            RateLimiter = BloFinExchange.RateLimiter.BloFinSocket;
            // Send a ping before the documented 30-second idle timeout.
            RegisterPeriodicQuery("Ping", TimeSpan.FromSeconds(25), _ => new BloFinPingQuery(), (connection, result) =>
            {
                if (result.Error?.ErrorType == ErrorType.Timeout)
                    _ = connection.TriggerReconnectAsync();
            });
        }

        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new BloFinSocketFuturesMessageConverter();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => BloFinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BloFinTrade[]>> onMessage, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BloFinTrade[]>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, int, BloFinSocketUpdate<BloFinTrade[]>>((receiveTime, originalData, invocations, data) =>
            {
                var timestamp = data.Data.Max(x => x.Timestamp);
                if (data.Action != "snapshot")
                    UpdateTimeOffset(timestamp);

                onMessage(new DataEvent<BloFinTrade[]>(Exchange, data.Data, receiveTime, originalData)
                    .WithUpdateType(data.Action == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                    .WithStreamId("trades")
                    .WithSymbol(data.Data.First().Symbol)
                    .WithDataTimestamp(timestamp, GetTimeOffset()));
            });
            var subscription = new BloFinSubscription<BloFinTrade[]>(_logger, this, "trades", symbols.ToArray(), handler, false, "SPOT");
            return await SubscribeAsync(BaseAddress.AppendPath("ws/spot/public"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BloFinSpotTicker>> onMessage, CancellationToken ct = default)
            => SubscribeToTickerUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BloFinSpotTicker>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DateTime, string?, int, BloFinSocketUpdate<BloFinSpotTicker[]>>((receiveTime, originalData, invocations, data) =>
            {
                var item = data.Data.First();
                if (data.Action != "snapshot")
                    UpdateTimeOffset(item.Timestamp);

                onMessage(new DataEvent<BloFinSpotTicker>(Exchange, item, receiveTime, originalData)
                    .WithUpdateType(data.Action == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                    .WithStreamId("tickers")
                    .WithSymbol(item.Symbol)
                    .WithDataTimestamp(item.Timestamp, GetTimeOffset()));
            });
            var subscription = new BloFinSubscription<BloFinSpotTicker[]>(_logger, this, "tickers", symbols.ToArray(), handler, false, "SPOT");
            return await SubscribeAsync(BaseAddress.AppendPath("ws/spot/public"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BloFinSpotKline>> onMessage, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync([symbol], interval, onMessage, ct);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BloFinSpotKline>> onMessage, CancellationToken ct = default)
        {
            var streamId = "candle" + EnumConverter.GetString(interval);
            var handler = new Action<DateTime, string?, int, BloFinSocketUpdate<BloFinSpotKline[]>>((receiveTime, originalData, invocations, data) =>
            {
                onMessage(new DataEvent<BloFinSpotKline>(Exchange, data.Data.First(), receiveTime, originalData)
                    .WithUpdateType(data.Action == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                    .WithStreamId(streamId)
                    .WithSymbol(data.Parameters.TryGetValue("instId", out var symbol) ? symbol : null));
            });
            var subscription = new BloFinSubscription<BloFinSpotKline[]>(_logger, this, streamId, symbols.ToArray(), handler, false, "SPOT");
            return await SubscribeAsync(BaseAddress.AppendPath("ws/spot/public"), subscription, ct).ConfigureAwait(false);
        }
    }
}
