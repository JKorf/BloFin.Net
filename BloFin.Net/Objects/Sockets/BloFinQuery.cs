using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using System.Text.Json;
using System.Linq;
using System;

namespace BloFin.Net.Objects.Sockets
{
    internal class BloFinQuery : Query<BloFinSocketResponse>
    {
        private readonly SocketApiClient _client;
        private readonly string _topic;
        private readonly HashSet<string>? _symbols;

        public BloFinQuery(SocketApiClient client, BloFinSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            _client = client;
            var listenList = new List<string>()
            {
                "error"
            };

            _topic = request.Parameters.First()["channel"]!;

            foreach (var param in request.Parameters)
            {
                if (param.TryGetValue("instId", out var symbol))
                {
                    _symbols ??= new HashSet<string>();
                    _symbols.Add(symbol);
                }

                var listenKey = request.Operation;
                if (param.TryGetValue("channel", out var channel))
                    listenKey += channel;
                if (symbol != null)
                    listenKey += symbol;
                listenList.Add(listenKey);
            }

            RequiredResponses = request.Parameters.Length;

            MessageMatcher = MessageMatcher.Create<BloFinSocketResponse>(listenList, HandleMessage);
        }

        public override bool PreCheckMessage(SocketConnection connection, object message)
        {
            var messageData = (BloFinSocketResponse)message;
            if (messageData.Code == 0)
                return true;

            if (_symbols == null)
                return true;

            // Check if the error response is for this query, we have to parse the original send request to see if it's this one
            if (!messageData.Message!.StartsWith("Invalid request: "))
                return true;

            var requestData = messageData.Message.Substring(17);
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
            var data = JsonSerializer.Deserialize<BloFinSocketRequest>(requestData, BloFinExchange._serializerContext)!;
#pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code

            if (data.Parameters.Any(x => x["channel"].ToString() == _topic && _symbols.Contains(x["instId"].ToString()!)))
            {
                // If this is an error response we only get this response
                RequiredResponses = 1;
                return true;
            }

            return false;
        }

        public CallResult<BloFinSocketResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BloFinSocketResponse message)
        {
            if (message.Code != 0)
                return new CallResult<BloFinSocketResponse>(new ServerError(message.Code, _client.GetErrorInfo(message.Code, message.Message)), originalData);

            return new CallResult<BloFinSocketResponse>(message, originalData, null);
        }
    }
}
