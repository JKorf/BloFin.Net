using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using System.Linq;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;

namespace BloFin.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BloFinSubscription<T> : Subscription
    {
        private readonly SocketApiClient _client;
        private readonly Action<DateTime, string?, int, BloFinSocketUpdate<T>> _handler;
        private readonly string _topic;
        private readonly string[]? _symbols;
        private readonly string? _symbolType;

        /// <summary>
        /// ctor
        /// </summary>
        public BloFinSubscription(
            ILogger logger,
            SocketApiClient client, 
            string topic,
            string[]? symbols,
            Action<DateTime, string?, int, BloFinSocketUpdate<T>> handler,
            bool auth,
            string? symbolType = null) : base(logger, auth)
        {
            _client = client;
            _handler = handler;
            _topic = topic;
            _symbols = symbols;
            _symbolType = symbolType;

            IndividualSubscriptionCount = symbols?.Length ?? 1;

            if (symbols == null)
                MessageRouter = MessageRouter.CreateForEvent<BloFinSocketUpdate<T>>(topic, DoHandleMessage);
            else
                MessageRouter = MessageRouter.CreateForEvent<BloFinSocketUpdate<T>>(topic, symbols, DoHandleMessage);
        }

        private Dictionary<string, string> CreateParameters(string? symbol)
        {
            var parameters = new Dictionary<string, string> { { "channel", _topic } };
            if (symbol != null)
                parameters.Add("instId", symbol);
            if (_symbolType != null)
                parameters.Add("instType", _symbolType);
            return parameters;
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new BloFinQuery(_client, new BloFinSocketRequest
            {
                Operation = "subscribe",
                Parameters = _symbols != null ? _symbols.Select(CreateParameters).ToArray() : [CreateParameters(null)]
            }, Authenticated);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new BloFinQuery(_client, new BloFinSocketRequest
            {
                Operation = "unsubscribe",
                Parameters = _symbols != null ? _symbols.Select(CreateParameters).ToArray() : [CreateParameters(null)]
            }, Authenticated);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BloFinSocketUpdate<T> message)
        {
            _handler(receiveTime, originalData, ConnectionInvocations, message);
            return CallResult.Ok();
        }
    }
}
