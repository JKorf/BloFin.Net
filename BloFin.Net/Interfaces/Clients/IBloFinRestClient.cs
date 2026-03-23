using BloFin.Net.Interfaces.Clients.Apis;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Options;

namespace BloFin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BloFin Rest API. 
    /// </summary>
    public interface IBloFinRestClient : IRestClient<BloFinCredentials>
    {
        /// <summary>
        /// General account API endpoints
        /// </summary>
        /// <see cref="IBloFinRestClientAccountApi"/>
        public IBloFinRestClientAccountApi AccountApi { get; }
        /// <summary>
        /// Futures API endpoints
        /// </summary>
        /// <see cref="IBloFinRestClientFuturesApi"/>
        public IBloFinRestClientFuturesApi FuturesApi { get; }
    }
}
