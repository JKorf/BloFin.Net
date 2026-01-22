using CryptoExchange.Net.Sockets;
using System;

namespace BloFin.Net.Objects.Sockets
{
    internal class BloFinPingQuery : Query<string>
    {

        public BloFinPingQuery() : base("ping", false, 1)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
            MessageRouter = MessageRouter.CreateWithoutHandler<string>("pong");
        }
    }
}
