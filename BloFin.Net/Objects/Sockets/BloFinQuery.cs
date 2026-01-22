using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using System.Text.Json;
using System.Linq;
using System;
using CryptoExchange.Net.Sockets.Default;

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

            var routes = new List<MessageRoute>();
            routes.Add(MessageRoute<BloFinSocketResponse>.CreateWithoutTopicFilter("error", HandleError, true));
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

                routes.Add(MessageRoute<BloFinSocketResponse>.CreateWithOptionalTopicFilter(request.Operation + param["channel"], symbol, HandleMessage));
            }

            RequiredResponses = request.Parameters.Length;

            MessageRouter = MessageRouter.Create(routes.ToArray());

        }

        public CallResult<BloFinSocketResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BloFinSocketResponse message)
        {
            if (message.Code != 0)
                return new CallResult<BloFinSocketResponse>(new ServerError(message.Code, _client.GetErrorInfo(message.Code, message.Message)), originalData);

            return new CallResult<BloFinSocketResponse>(message, originalData, null);
        }

        public CallResult<BloFinSocketResponse>? HandleError(SocketConnection connection, DateTime receiveTime, string? originalData, BloFinSocketResponse message)
        {
            if (message.Code == 0)
                return null;

            if (_symbols == null)
                return null;

            // Check if the error response is for this query, we have to parse the original send request to see if it's this one
            if (!message.Message!.StartsWith("Invalid request: "))
                return new CallResult<BloFinSocketResponse>(message);

            var requestData = message.Message.Substring(17);
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
            var data = JsonSerializer.Deserialize<BloFinSocketRequest>(requestData, BloFinExchange._serializerContext)!;
#pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code

            if (data.Parameters.Any(x => x["channel"].ToString() == _topic && _symbols.Contains(x["instId"].ToString()!)))
            {
                // If this is an error response we only get this response
                RequiredResponses = 1;
                return new CallResult<BloFinSocketResponse>(new ServerError(message.Code, _client.GetErrorInfo(message.Code, message.Message)));
            }

            return null;
        }
    }
}
