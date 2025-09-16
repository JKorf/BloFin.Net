using Microsoft.Extensions.Logging;
using System.Net.Http;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Objects.Errors;
using BloFin.Net.Interfaces.Clients.FuturesApi;

namespace BloFin.Net.Clients.FuturesApi
{
    /// <inheritdoc cref="IBloFinRestClientFuturesApi" />
    internal partial class BloFinRestClientFuturesApi : BloFinRestClientApi, IBloFinRestClientFuturesApi
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
        #endregion

        #region constructor/destructor
        internal BloFinRestClientFuturesApi(ILogger logger, HttpClient? httpClient, BloFinRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
            Account = new BloFinRestClientFuturesApiAccount(this);
            ExchangeData = new BloFinRestClientFuturesApiExchangeData(logger, this);
            Trading = new BloFinRestClientFuturesApiTrading(logger, this);
        }
        #endregion

        /// <inheritdoc />
        public IBloFinRestClientFuturesApiShared SharedClient => this;
    }
}
