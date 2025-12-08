using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using System;
using CryptoExchange.Net.Sockets.Default;

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
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<BloFinSocketResponse>(listenList, HandleMessage);
        }

        public CallResult<BloFinSocketResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BloFinSocketResponse message)
        {
            if (message.Code != 0)
                return new CallResult<BloFinSocketResponse>(new ServerError(message.Code, _client.GetErrorInfo(message.Code, message.Message)), originalData);

            return new CallResult<BloFinSocketResponse>(message, originalData, null);
        }
    }
}
