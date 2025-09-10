using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using BloFin.Net.Objects.Models;

namespace BloFin.Net.Objects.Sockets
{
    internal class BloFinQuery<T> : Query<T>
    {
        public BloFinQuery(BloFinModel request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            MessageMatcher = MessageMatcher.Create<T>("", HandleMessage);
        }

        public CallResult<T> HandleMessage(SocketConnection connection, DataEvent<T> message)
        {
            return message.ToCallResult();
        }
    }
}
