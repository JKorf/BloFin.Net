using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloFin.Net
{
    internal class BloFinAuthenticationProvider : AuthenticationProvider
    {
        private static readonly IMessageSerializer _serializer = new SystemTextJsonMessageSerializer(BloFinExchange._serializerContext);

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

        public Dictionary<string, string> GetSocketParameters()
        {
            var timestamp = DateTimeConverter.ConvertToMilliseconds(DateTime.UtcNow).ToString()!;
            var nonce = Guid.NewGuid().ToString();

            var signStr = $"/users/self/verifyGET{timestamp}{nonce}";
            var sign = SignHMACSHA256(signStr, SignOutputType.Hex).ToLowerInvariant();
            sign = Convert.ToBase64String(Encoding.UTF8.GetBytes(sign));

            return new Dictionary<string, string>
            {
                { "apiKey", ApiKey },
                { "passphrase", Pass! },
                { "nonce", nonce },
                { "timestamp", timestamp },
                { "sign", sign }
            };
        }
    }
}
