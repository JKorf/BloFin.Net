using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace BloFin.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BloFin Spot REST API endpoints
    /// </summary>
    public interface IBloFinRestClientSpotApi : IRestApiClient<BloFinCredentials>, IDisposable
    {
        /// <summary>
        /// User account endpoints
        /// </summary>
        IBloFinRestClientSpotApiAccount Account { get; }
        /// <summary>
        /// Public spot market data endpoints
        /// </summary>
        IBloFinRestClientSpotApiExchangeData ExchangeData { get; }
        /// <summary>
        /// Trading endpoints
        /// </summary>
        IBloFinRestClientSpotApiTrading Trading { get; }
        /// <summary>
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        public IBloFinRestClientSpotSharedApi SharedApi { get; }
    }
}
