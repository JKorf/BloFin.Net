using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;

namespace BloFin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the BloFin websocket API
    /// </summary>
    public interface IBloFinSocketClient : ISocketClient<BloFinCredentials>
    {
        /// <summary>
        /// Spot market data streams
        /// </summary>
        IBloFinSocketClientSpotApi SpotApi { get; }
        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        /// <see cref="IBloFinSocketClientFuturesApi"/>
        public IBloFinSocketClientFuturesApi FuturesApi { get; }
    }
}
