using BloFin.Net.Objects.Internal;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients
{
    /// <summary>
    /// BloFin rest client base
    /// </summary>
    public abstract class BloFinRestClientApi : RestApiClient
    {
        internal new BloFinRestOptions ClientOptions => (BloFinRestOptions)base.ClientOptions;

        #region Api clients
        /// <inheritdoc />
        public string ExchangeName => "BloFin";
        #endregion

        #region constructor/destructor
        internal BloFinRestClientApi(ILogger logger, HttpClient? httpClient, string address, BloFinRestOptions options, RestApiOptions apiOptions)
            : base(logger, httpClient, address, options, apiOptions)
        {
        }
        #endregion

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
            => BloFinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(BloFinExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(BloFinExchange._serializerContext);


        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BloFinAuthenticationProvider(credentials);

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<BloFinResponse>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            return result.AsDataless();
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<BloFinResponse<T>>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            return result.As<T>(result.Data?.Data);
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => throw new NotImplementedException();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo() => null;

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset() => null;

    }
}
