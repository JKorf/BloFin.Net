using CryptoExchange.Net.SharedApis;

namespace BloFin.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IBloFinRestClientSpotSharedApi :
        IGetSpotSymbolsRest,
        IGetTickerRest,
        IGetAllTickersRest,
        IGetOrderBookRest,
        IGetRecentTradesRest,
        IGetKlinesRest,
        IGetBalancesRest,
        ITransferRest,
        IGetTransferHistoryRest,
        IPlaceSpotOrderRest,
        IPlaceMultipleSpotOrdersRest,
        ICancelSpotOrderRest,
        ICancelSpotOrderByClientOrderIdRest,
        IGetOpenSpotOrdersRest,
        IGetClosedSpotOrdersRest,
        IGetSpotOrderRest,
        IGetSpotOrderByClientOrderIdRest,
        IGetSpotUserTradeHistoryRest,
        IGetSpotOrderTradesRest
    { }
}
