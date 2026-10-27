using BloFin.Net.Interfaces.Clients.AccountApi;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.SharedApis;

namespace BloFin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of BloFin
    /// </summary>
    public interface IBloFinSharedApiClient : ISharedApiClientBase
    {
        /// <summary>
        /// Account REST shared API implementations
        /// </summary>
        IBloFinRestClientAccountSharedApi AccountRest { get; }

        /// <summary>
        /// Futures REST shared API implementations
        /// </summary>
        IBloFinRestClientFuturesSharedApi FuturesRest { get; }

        /// <summary>
        /// Futures WebSocket shared API implementations
        /// </summary>
        IBloFinSocketClientFuturesSharedApi FuturesSocket { get; }

        /// <summary>
        /// Spot REST Shared API implementations
        /// </summary>
        public IBloFinRestClientSpotSharedApi SpotRest { get; }
        /// <summary>
        /// Spot WebSocket Shared API implementations
        /// </summary>
        public IBloFinSocketClientSpotSharedApi SpotSocket { get; }
    }
}
