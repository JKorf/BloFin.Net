using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using BloFin.Net.Objects.Models;
using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using System.Text.Json;
using System.Linq;

namespace BloFin.Net.Objects.Sockets
{
    internal class BloFinQuery : Query<BloFinSocketResponse>
    {
        private readonly SocketApiClient _client;
        private readonly string _topic;
        private readonly HashSet<string> _symbols = new();

        public BloFinQuery(SocketApiClient client, BloFinSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            _client = client;
            var listenList = new List<string>()
            {
                "error"
            };

            _topic = request.Parameters.First()["channel"].ToString()!;

            foreach (var param in request.Parameters)
            {
                _symbols.Add(param["instId"].ToString()!);

                var listenKey = request.Operation;
                if (param.TryGetValue("channel", out var channel))
                    listenKey += channel;
                if (param.TryGetValue("instId", out var instId))
                    listenKey += instId;
                listenList.Add(listenKey);
            }

            RequiredResponses = request.Parameters.Length;

            MessageMatcher = MessageMatcher.Create<BloFinSocketResponse>(listenList, HandleMessage);
        }

        public override bool PreCheckMessage(SocketConnection connection, DataEvent<object> message)
        {
            var messageData = (BloFinSocketResponse)message.Data;
            if (messageData.Code == 0)
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

        public CallResult<BloFinSocketResponse> HandleMessage(SocketConnection connection, DataEvent<BloFinSocketResponse> message)
        {
            if (message.Data.Code != 0)
                return new CallResult<BloFinSocketResponse>(new ServerError(message.Data.Code, _client.GetErrorInfo(message.Data.Code, message.Data.Message)));

            return message.ToCallResult();
        }
    }
}
