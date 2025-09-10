using CryptoExchange.Net.Interfaces;
using System;

namespace BloFin.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// BloFin Exchange API endpoints
    /// </summary>
    public interface IBloFinRestClientExchangeApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IBloFinRestClientExchangeApiAccount" />
        public IBloFinRestClientExchangeApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IBloFinRestClientExchangeApiExchangeData" />
        public IBloFinRestClientExchangeApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IBloFinRestClientExchangeApiTrading" />
        public IBloFinRestClientExchangeApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IBloFinRestClientExchangeApiShared SharedClient { get; }
    }
}
