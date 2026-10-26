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
        /// Public spot market data endpoints
        /// </summary>
        IBloFinRestClientSpotApiExchangeData ExchangeData { get; }
    }
}
