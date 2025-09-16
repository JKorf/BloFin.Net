using CryptoExchange.Net.SharedApis;

namespace BloFin.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Shared interface for Exchange rest API usage
    /// </summary>
    public interface IBloFinRestClientFuturesApiShared :
        IBookTickerRestClient,
        IKlineRestClient,
        IOrderBookRestClient,
        IRecentTradeRestClient,
        IFundingRateRestClient,
        IFuturesSymbolRestClient,
        IFuturesTickerRestClient,
        IIndexPriceKlineRestClient,
        IMarkPriceKlineRestClient,
        IBalanceRestClient,
        IPositionModeRestClient,
        ILeverageRestClient,
        IFuturesOrderRestClient,
        IFuturesOrderClientIdRestClient,
        IFuturesTpSlRestClient,
        IFuturesTriggerOrderRestClient
    {
    }
}
