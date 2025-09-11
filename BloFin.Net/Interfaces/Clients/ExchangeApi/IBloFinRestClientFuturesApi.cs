using CryptoExchange.Net.Interfaces;
using System;

namespace BloFin.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// BloFin Exchange API endpoints
    /// </summary>
    public interface IBloFinRestClientFuturesApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IBloFinRestClientFuturesApiAccount" />
        public IBloFinRestClientFuturesApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IBloFinRestClientFuturesApiExchangeData" />
        public IBloFinRestClientFuturesApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IBloFinRestClientFuturesApiTrading" />
        public IBloFinRestClientFuturesApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IBloFinRestClientFuturesApiShared SharedClient { get; }
    }
}
