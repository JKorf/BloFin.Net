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
        IFuturesTriggerOrderRestClient,
        IPositionHistoryRestClient
    {
    }

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IBloFinRestClientFuturesSharedApi :
        IGetBookTickerRest,
        IGetKlinesRest,
        IGetOrderBookRest,
        IGetRecentTradesRest,
        IGetFundingRateHistoryRest,
        IGetFuturesSymbolsRest,
        IGetFuturesTickerRest,
        IGetAllFuturesTickersRest,
        IGetIndexPriceKlinesRest,
        IGetMarkPriceKlinesRest,
        IGetBalancesRest,
        IGetPositionModeRest,
        ISetPositionModeRest,
        IGetLeverageRest,
        ISetLeverageRest,
        IPlaceFuturesOrderRest,
        IGetFuturesOrderRest,
        IGetOpenFuturesOrdersRest,
        IGetClosedFuturesOrdersRest,
        IGetFuturesOrderTradesRest,
        IGetFuturesUserTradeHistoryRest,
        ICancelFuturesOrderRest,
        IGetPositionsRest,
        IClosePositionRest,
        IGetFuturesOrderByClientOrderIdRest,
        ICancelFuturesOrderByClientOrderIdRest,
        ISetFuturesTpSlRest,
        ICancelFuturesTpSlRest,
        IPlaceFuturesTriggerOrderRest,
        IGetFuturesTriggerOrderRest,
        ICancelFuturesTriggerOrderRest,
        IGetPositionHistoryRest
    { }
}
