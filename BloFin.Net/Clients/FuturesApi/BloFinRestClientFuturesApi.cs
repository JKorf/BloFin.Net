using BloFin.Net.Clients.MessageHandlers;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Objects.Errors;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace BloFin.Net.Clients.FuturesApi
{
    /// <inheritdoc cref="IBloFinRestClientFuturesApi" />
    internal partial class BloFinRestClientFuturesApi : BloFinRestClientApi, IBloFinRestClientFuturesApi
    {
        #region fields 
        protected override ErrorMapping ErrorMapping => BloFinErrors.Errors;
        protected override IRestMessageHandler MessageHandler { get; } = new BloFinRestMessageHandler(BloFinErrors.Errors);

        public new BloFinRestOptions ClientOptions => (BloFinRestOptions)base.ClientOptions;
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
