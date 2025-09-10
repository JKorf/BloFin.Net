using BloFin.Net.Interfaces.Clients.ExchangeApi;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;

namespace BloFin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BloFin websocket API
    /// </summary>
    public interface IBloFinSocketClient : ISocketClient
    {        
        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        /// <see cref="IBloFinSocketClientExchangeApi"/>
        public IBloFinSocketClientExchangeApi ExchangeApi { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
