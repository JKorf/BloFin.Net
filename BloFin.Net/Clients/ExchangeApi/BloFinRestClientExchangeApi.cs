using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BloFin.Net.Interfaces.Clients.ExchangeApi;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;
using BloFin.Net.Objects.Internal;

namespace BloFin.Net.Clients.ExchangeApi
{
    /// <inheritdoc cref="IBloFinRestClientExchangeApi" />
    internal partial class BloFinRestClientExchangeApi : RestApiClient, IBloFinRestClientExchangeApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Exchange Api");

        protected override ErrorMapping ErrorMapping => BloFinErrors.Errors;
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IBloFinRestClientExchangeApiAccount Account { get; }
        /// <inheritdoc />
        public IBloFinRestClientExchangeApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBloFinRestClientExchangeApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "BloFin";
        #endregion

        #region constructor/destructor
        internal BloFinRestClientExchangeApi(BloFinRestClient client, ILogger logger, HttpClient? httpClient, BloFinRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
            Account = new BloFinRestClientExchangeApiAccount(this);
            ExchangeData = new BloFinRestClientExchangeApiExchangeData(logger, this);
            Trading = new BloFinRestClientExchangeApiTrading(logger, this);
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
#warning TODO if API returns errors on HttpStatus 200 this should return it for correct logging
            return null;
        }

        protected override Error ParseErrorResponse(int httpStatusCode, KeyValuePair<string, string[]>[] responseHeaders, IMessageAccessor accessor, Exception? exception) {
#warning TODO if API returns errors on HttpStatus != 200 this should parse them
            return base.ParseErrorResponse(httpStatusCode, responseHeaders, accessor, exception);
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => null;// ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp, ApiOptions.TimestampRecalculationInterval ?? ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => BloFinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public IBloFinRestClientExchangeApiShared SharedClient => this;

    }
}
