using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;

namespace BloFin.Net.Objects.Sockets
{
    internal class BloFinLoginQuery : Query<BloFinSocketResponse>
    {
        private readonly SocketApiClient _client;

        public BloFinLoginQuery(SocketApiClient client, Dictionary<string, string> request, bool authenticated, int weight = 1) : base(
            new BloFinSocketRequest { Operation = "login", Parameters = [request] }, authenticated, weight)
        {
            _client = client;
            var listenList = new List<string>()
            {
                "error",
                "login"
            };

            MessageMatcher = MessageMatcher.Create<BloFinSocketResponse>(listenList, HandleMessage);
        }

        public CallResult<BloFinSocketResponse> HandleMessage(SocketConnection connection, DataEvent<BloFinSocketResponse> message)
        {
            if (message.Data.Code != 0)
                return new CallResult<BloFinSocketResponse>(new ServerError(message.Data.Code, _client.GetErrorInfo(message.Data.Code, message.Data.Message)));

            return message.ToCallResult();
        }
    }
}
