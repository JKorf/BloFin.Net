using BloFin.Net.Clients.MessageHandlers;
using BloFin.Net.Interfaces.Clients.SpotApi;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Objects.Errors;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace BloFin.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IBloFinRestClientSpotApi" />
    internal class BloFinRestClientSpotApi : BloFinRestClientApi, IBloFinRestClientSpotApi
    {
        /// <inheritdoc />
        protected override ErrorMapping ErrorMapping => BloFinErrors.Errors;
        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new BloFinRestMessageHandler(BloFinErrors.Errors);

        /// <summary>
        /// Create the spot REST API client
        /// </summary>
        internal BloFinRestClientSpotApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, BloFinRestOptions options)
            : base(loggerFactory, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
            ExchangeData = new BloFinRestClientSpotApiExchangeData(this);
        }

        /// <inheritdoc />
        public IBloFinRestClientSpotApiExchangeData ExchangeData { get; }
    }
}
