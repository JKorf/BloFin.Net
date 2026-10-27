using BloFin.Net.Interfaces.Clients.FuturesApi;
using CryptoExchange.Net.SharedApis;

namespace BloFin.Net.Clients.FuturesApi
{
    internal partial class BloFinRestClientFuturesSharedApi :
        SharedApiBase,
        IBloFinRestClientFuturesApiShared,
        IBloFinRestClientFuturesSharedApi
    {
        private readonly BloFinRestClientFuturesApi _api;

        private const string _topicId = "BloFinFutures";
        private const string _exchangeName = "BloFin";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BloFinExchange.Metadata, this);

        public BloFinRestClientFuturesSharedApi(BloFinRestClientFuturesApi api)
            : base(
                  SharedTransport.Rest,
                  api,
                  new[] { TradingMode.PerpetualLinear, TradingMode.PerpetualInverse },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetBookTickerOptions,
                GetKlinesOptions,
                GetOrderBookOptions,
                GetRecentTradesOptions,
                GetFundingRateHistoryOptions,
                GetFuturesSymbolsOptions,
                GetTickerOptions,
                GetAllTickersOptions,
                GetIndexPriceKlinesOptions,
                GetMarkPriceKlinesOptions,
                GetBalancesOptions,
                GetPositionModeOptions,
                SetPositionModeOptions,
                GetLeverageOptions,
                SetLeverageOptions,
                PlaceFuturesOrderOptions,
                GetFuturesOrderOptions,
                GetOpenFuturesOrdersOptions,
                GetClosedFuturesOrdersOptions,
                GetFuturesOrderTradesOptions,
                GetFuturesUserTradeHistoryOptions,
                CancelFuturesOrderOptions,
                GetPositionsOptions,
                CloseFullPositionOptions,
                GetFuturesOrderByClientOrderIdOptions,
                CancelFuturesOrderByClientOrderIdOptions,
                SetFuturesTpSlOptions,
                CancelFuturesTpSlOptions,
                PlaceFuturesTriggerOrderOptions,
                GetFuturesTriggerOrderOptions,
                CancelFuturesTriggerOrderOptions,
                GetPositionHistoryOptions,
                GetMarkPriceOptions,
                GetAllMarkPricesOptions,
                GetFundingInfoOptions,
                GetLeverageTiersOptions,
                PlaceMultipleFuturesOrdersOptions,
                EditFuturesOrderOptions,
                EditFuturesOrderByClientOrderIdOptions,
                GetAllAssetsOptions
            );
        }
    }
}
