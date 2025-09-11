using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System;

namespace BloFin.Net.Objects.Sockets
{
    internal class BloFinPingQuery : Query<string>
    {

        public BloFinPingQuery() : base("ping", false, 1)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);

            MessageMatcher = MessageMatcher.Create<string>("pong", HandleMessage);
        }

        public CallResult<string> HandleMessage(SocketConnection connection, DataEvent<string> message)
        {
            return message.ToCallResult();
        }
    }
}
