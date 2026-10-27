using CryptoExchange.Net.SharedApis;

namespace BloFin.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IBloFinSocketClientSpotSharedApi :
        ISubscribeTradesSocket,
        ISubscribeTickerSocket,
        ISubscribeIncrementalOrderBookSocket,
        ISubscribeOrderBookSocket,
        ISubscribeKlinesSocket,
        ISubscribeBalancesSocket,
        ISubscribeSpotOrdersSocket
    { }
}
