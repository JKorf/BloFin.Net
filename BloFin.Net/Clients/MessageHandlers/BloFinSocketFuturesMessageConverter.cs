using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using System;
using System.Net.WebSockets;
using System.Text.Json;

namespace BloFin.Net.Clients.MessageHandlers
{
    internal class BloFinSocketFuturesMessageConverter : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = BloFinExchange._serializerContext;

        public BloFinSocketFuturesMessageConverter()
        {
            AddTopicMapping<BloFinSocketResponse>(x => x.Parameters.TryGetValue("instId", out var symbol) ? symbol : null);
            AddTopicMapping<BloFinSocketUpdate>(x => x.Parameters.TryGetValue("instId", out var symbol) ? symbol : null);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("event"),
                    new PropertyFieldReference("channel") { Depth = 2 },
                ],
                TypeIdentifierCallback = x => x.FieldValue("event") + x.FieldValue("channel")
            },

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("event"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("event")!
            },

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("channel") { Depth = 2 },
                ],
                TypeIdentifierCallback = x => x.FieldValue("channel")!
            },
        ];

        public override string? GetTypeIdentifier(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "pong";

            return base.GetTypeIdentifier(data, webSocketMessageType);
        }
    }
}
