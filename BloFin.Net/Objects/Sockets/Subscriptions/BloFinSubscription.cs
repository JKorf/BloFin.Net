using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using BloFin.Net.Objects.Models;
using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using System.Linq;

namespace BloFin.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class BloFinSubscription<T> : Subscription<BloFinSocketResponse, BloFinSocketResponse>
    {
        private readonly SocketApiClient _client;
        private readonly Action<DataEvent<T>> _handler;
        private readonly string _topic;
        private readonly string[] _symbols;

        /// <summary>
        /// ctor
        /// </summary>
        public BloFinSubscription(ILogger logger, SocketApiClient client, string topic, string[] symbols, Action<DataEvent<T>> handler, bool auth) : base(logger, auth)
        {
            _client = client;
            _handler = handler;
            _topic = topic;
            _symbols = symbols;

            MessageMatcher = MessageMatcher.Create<BloFinSocketUpdate<T>>(symbols.Select(x => topic + x), DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new BloFinQuery(_client, new BloFinSocketRequest
            {
                Operation = "subscribe",
                Parameters = _symbols.Select(x => new Dictionary<string, object>
                {
                    { "channel", _topic },
                    { "instId", x }
                }).ToArray()                
            }, Authenticated);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new BloFinQuery(_client, new BloFinSocketRequest
            {
                Operation = "unsubscribe",
                Parameters = _symbols.Select(x => new Dictionary<string, object>
                {
                    { "channel", _topic },
                    { "instId", x }
                }).ToArray()
            }, Authenticated);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<BloFinSocketUpdate<T>> message)
        {
            _handler.Invoke(message.As(message.Data.Data, _topic, message.Data.Parameters["instId"].ToString(), message.Data.Action == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}
