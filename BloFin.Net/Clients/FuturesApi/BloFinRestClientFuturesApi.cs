using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;
using BloFin.Net.Objects.Internal;
using CryptoExchange.Net.Converters.MessageParsing;
using BloFin.Net.Interfaces.Clients.FuturesApi;

namespace BloFin.Net.Clients.FuturesApi
{
    /// <inheritdoc cref="IBloFinRestClientFuturesApi" />
    internal partial class BloFinRestClientFuturesApi : RestApiClient, IBloFinRestClientFuturesApi
    {
        #region fields 
        protected override ErrorMapping ErrorMapping => BloFinErrors.Errors;
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IBloFinRestClientFuturesApiAccount Account { get; }
        /// <inheritdoc />
        public IBloFinRestClientFuturesApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBloFinRestClientFuturesApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "BloFin";
        #endregion

        #region constructor/destructor
        internal BloFinRestClientFuturesApi(BloFinRestClient client, ILogger logger, HttpClient? httpClient, BloFinRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
            Account = new BloFinRestClientFuturesApiAccount(this);
            ExchangeData = new BloFinRestClientFuturesApiExchangeData(logger, this);
            Trading = new BloFinRestClientFuturesApiTrading(logger, this);
        }
        #endregion

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

            // Optional response checking

            return result.AsDataless();
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<BloFinResponse<T>>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            
            return result.As<T>(result.Data?.Data);
        }

        protected override Error? TryParseError(RequestDefinition definition, KeyValuePair<string, string[]>[] responseHeaders, IMessageAccessor accessor) {
            var responseCode = accessor.GetValue<int>(MessagePath.Get().Property("code"));
            if (responseCode >= 0 && responseCode <= 2) // 0 = success, 1 = failed, 2 = partial success. More details in response so process them further
                return null;

            return new ServerError(responseCode, GetErrorInfo(responseCode, accessor.GetValue<string>(MessagePath.Get().Property("msg"))));
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => throw new NotImplementedException();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo() => null;

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset() => null;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => BloFinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public IBloFinRestClientFuturesApiShared SharedClient => this;

    }
}
