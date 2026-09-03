using CryptoExchange.Net.SharedApis;

namespace BloFin.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Shared interface for Exchange socket API usage
    /// </summary>
    public interface IBloFinSocketClientFuturesApiShared :
        IBookTickerSocketClient,
        IKlineSocketClient,
        IOrderBookSocketClient,
        ITickerSocketClient,
        ITradeSocketClient,
        IBalanceSocketClient,
        IFuturesOrderSocketClient,
        IPositionSocketClient
    {
    }

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IBloFinSocketClientFuturesSharedApi :
        ISubscribeBookTickerSocket,
        ISubscribeKlinesSocket,
        ISubscribeOrderBookSocket,
        ISubscribeTickerSocket,
        ISubscribeTradesSocket,
        ISubscribeBalancesSocket,
        ISubscribeFuturesOrdersSocket,
        ISubscribePositionsSocket
    { }
}
