using BloFin.Net.Interfaces.Clients.AccountApi;
using BloFin.Net.Interfaces.Clients.FuturesApi;

namespace BloFin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of BloFin
    /// </summary>
    public interface IBloFinSharedApiClient
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
    }
}
