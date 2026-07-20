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
    internal abstract class BloFinRestClientApi : RestApiClient<BloFinEnvironment, BloFinAuthenticationProvider, BloFinCredentials>
    {
        internal new BloFinRestOptions ClientOptions => (BloFinRestOptions)base.ClientOptions;

        #region Api clients
        /// <inheritdoc />
        public string ExchangeName => "BloFin";
        #endregion

        #region constructor/destructor
        internal BloFinRestClientApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, string address, BloFinRestOptions options, RestApiOptions apiOptions)
            : base(loggerFactory, BloFinExchange.Metadata.Id, httpClient, address, options, apiOptions)
        {
        }
        #endregion

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
            => BloFinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(BloFinExchange._serializerContext);

        /// <inheritdoc />
        protected override BloFinAuthenticationProvider CreateAuthenticationProvider(BloFinCredentials credentials)
            => new BloFinAuthenticationProvider(credentials);

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<BloFinResponse<T>>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<T>(result);

            if (result.Data.Code != 0 && result.Data.Code != 200)
                return HttpResult.Fail<T>(result, new ServerError(result.Data.Code, GetErrorInfo(result.Data.Code, result.Data.Message)), result.Data.Data);

            return HttpResult.Ok<T>(result, result.Data.Data);
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync()
            => throw new NotImplementedException();

    }
}
