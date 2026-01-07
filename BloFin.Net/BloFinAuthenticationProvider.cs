using BloFin.Net.Objects.Sockets;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloFin.Net
{
    internal class BloFinAuthenticationProvider : AuthenticationProvider
    {
        private static readonly IMessageSerializer _serializer = new SystemTextJsonMessageSerializer(BloFinExchange._serializerContext);

        public override ApiCredentialsType[] SupportedCredentialTypes => [ApiCredentialsType.Hmac];
        public BloFinAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
            if (credentials.Pass == null)
                throw new ArgumentException("Pass is required for BloFin authentication");
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.Authenticated)
                return;

            var timestamp = GetMillisecondTimestamp(apiClient);
            var nonce = Guid.NewGuid().ToString();

            var query = requestConfig.ParameterPosition == HttpMethodParameterPosition.InUri && (requestConfig.QueryParameters?.Count > 0) ? "?" + requestConfig.QueryParameters.CreateParamString(true, requestConfig.ArraySerialization) : "";
            if (requestConfig.ParameterPosition == HttpMethodParameterPosition.InUri)
                requestConfig.SetQueryString(query);

            var body = requestConfig.ParameterPosition == HttpMethodParameterPosition.InBody ? GetSerializedBody(_serializer, requestConfig.BodyParameters ?? new Dictionary<string, object>()) : "";
            if (requestConfig.ParameterPosition == HttpMethodParameterPosition.InBody)
                requestConfig.SetBodyContent(body);

            var signStr = $"{requestConfig.Path}{query}{requestConfig.Method}{timestamp}{nonce}{body}";
            var sign = SignHMACSHA256(signStr, SignOutputType.Hex).ToLowerInvariant();
            sign = Convert.ToBase64String(Encoding.UTF8.GetBytes(sign));

            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers.Add("ACCESS-KEY", ApiKey);
            requestConfig.Headers.Add("ACCESS-SIGN", sign);
            requestConfig.Headers.Add("ACCESS-TIMESTAMP", timestamp);
            requestConfig.Headers.Add("ACCESS-NONCE", nonce);
            requestConfig.Headers.Add("ACCESS-PASSPHRASE", Pass!);
        }

        public override Query? GetAuthenticationQuery(SocketApiClient apiClient, SocketConnection connection, Dictionary<string, object?>? context = null)
        {
            var timestamp = DateTimeConverter.ConvertToMilliseconds(DateTime.UtcNow).ToString()!;
            var nonce = Guid.NewGuid().ToString();

            var signStr = $"/users/self/verifyGET{timestamp}{nonce}";
            var sign = SignHMACSHA256(signStr, SignOutputType.Hex).ToLowerInvariant();
            sign = Convert.ToBase64String(Encoding.UTF8.GetBytes(sign));

            var parameters = new Dictionary<string, string>
            {
                { "apiKey", ApiKey },
                { "passphrase", Pass! },
                { "nonce", nonce },
                { "timestamp", timestamp },
                { "sign", sign }
            };

            return new BloFinLoginQuery(apiClient, parameters, false);
        }
    }
}
