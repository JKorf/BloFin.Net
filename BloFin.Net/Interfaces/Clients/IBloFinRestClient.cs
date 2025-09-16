using BloFin.Net.Interfaces.Clients.Apis;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Options;

namespace BloFin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BloFin Rest API. 
    /// </summary>
    public interface IBloFinRestClient : IRestClient
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

        /// <summary>
        /// Update specific options
        /// </summary>
        /// <param name="options">Options to update. Only specific options are changeable after the client has been created</param>
        void SetOptions(UpdateOptions options);

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
