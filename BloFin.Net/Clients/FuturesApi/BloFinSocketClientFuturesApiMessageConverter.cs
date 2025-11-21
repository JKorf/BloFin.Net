using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Net.WebSockets;
using System.Text.Json;

namespace BloFin.Net.Clients.FuturesApi
{
    internal class BloFinSocketClientFuturesApiMessageConverter : DynamicJsonConverter
    {
        public override JsonSerializerOptions Options { get; } = BloFinExchange._serializerContext;

        protected override MessageEvaluator[] MessageEvaluators { get; } = [

            new MessageEvaluator {
                Priority = 1,
                Fields = [
                    new PropertyFieldReference("event"),
                    new PropertyFieldReference("channel") { Depth = 2 },
                    new PropertyFieldReference("instId") { Depth = 2 },
                ],
                IdentifyMessageCallback = x => x.FieldValue("event") + x.FieldValue("channel") + x.FieldValue("instId")
            },

            new MessageEvaluator {
                Priority = 2,
                Fields = [
                    new PropertyFieldReference("event"),
                    new PropertyFieldReference("channel") { Depth = 2 },
                ],
                IdentifyMessageCallback = x => x.FieldValue("event") + x.FieldValue("channel")
            },

            new MessageEvaluator {
                Priority = 3,
                Fields = [
                    new PropertyFieldReference("channel") { Depth = 2 },
                    new PropertyFieldReference("instId") { Depth = 2 },
                ],
                IdentifyMessageCallback = x => x.FieldValue("channel") + x.FieldValue("instId")
            },

            new MessageEvaluator {
                Priority = 4,
                Fields = [
                    new PropertyFieldReference("event"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("event")
            },

            new MessageEvaluator {
                Priority = 5,
                Fields = [
                    new PropertyFieldReference("channel") { Depth = 2 },
                ],
                IdentifyMessageCallback = x => x.FieldValue("channel")
            },
        ];

        public override string? GetMessageIdentifier(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "pong";

            return base.GetMessageIdentifier(data, webSocketMessageType);
        }
    }
}
