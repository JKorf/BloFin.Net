using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using System.Linq;
using CryptoExchange.Net.Sockets.Default;

namespace BloFin.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BloFinSubscription<T> : Subscription
    {
        private readonly SocketApiClient _client;
        private readonly Action<DateTime, string?, int, BloFinSocketUpdate<T>> _handler;
        private readonly string _topic;
        private readonly string[]? _symbols;

        /// <summary>
        /// ctor
        /// </summary>
        public BloFinSubscription(
            ILogger logger,
            SocketApiClient client, 
            string topic,
            string[]? symbols,
            Action<DateTime, string?, int, BloFinSocketUpdate<T>> handler,
            bool auth) : base(logger, auth)
        {
            _client = client;
            _handler = handler;
            _topic = topic;
            _symbols = symbols;

            MessageMatcher = MessageMatcher.Create<BloFinSocketUpdate<T>>(symbols != null ? symbols.Select(x => topic + x) : [topic], DoHandleMessage);
            MessageRouter = MessageRouter.CreateWithOptionalTopicFilters<BloFinSocketUpdate<T>>(topic, symbols, DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new BloFinQuery(_client, new BloFinSocketRequest
            {
                Operation = "subscribe",
                Parameters = _symbols != null ? _symbols.Select(x => new Dictionary<string, string>
                {
                    { "channel", _topic },
                    { "instId", x }
                }).ToArray()
                :
                [new Dictionary<string, string>
                {
                    { "channel", _topic }
                }]
            }, Authenticated);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new BloFinQuery(_client, new BloFinSocketRequest
            {
                Operation = "unsubscribe",
                Parameters = _symbols != null ? _symbols.Select(x => new Dictionary<string, string>
                {
                    { "channel", _topic },
                    { "instId", x }
                }).ToArray()
                :
                [new Dictionary<string, string>
                {
                    { "channel", _topic }
                }]
            }, Authenticated);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BloFinSocketUpdate<T> message)
        {
            _handler(receiveTime, originalData, ConnectionInvocations, message);
            return new CallResult(null);
        }
    }
}
