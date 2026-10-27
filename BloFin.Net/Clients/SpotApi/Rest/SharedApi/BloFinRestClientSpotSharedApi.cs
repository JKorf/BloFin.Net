using BloFin.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.SharedApis;

namespace BloFin.Net.Clients.SpotApi
{
    internal partial class BloFinRestClientSpotSharedApi :
        SharedApiBase,
        IBloFinRestClientSpotSharedApi
    {
        private readonly BloFinRestClientSpotApi _api;

        private const string _topicId = "BloFinSpot";
        private const string _exchangeName = "BloFin";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BloFinExchange.Metadata, this);

        public BloFinRestClientSpotSharedApi(BloFinRestClientSpotApi api)
            : base(
                  SharedTransport.Rest,
                  api,
                  new[] { TradingMode.Spot },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetSpotSymbolsOptions,
                GetTickerOptions,
                GetAllTickersOptions,
                GetOrderBookOptions,
                GetRecentTradesOptions,
                GetKlinesOptions,
                GetBalancesOptions,
                TransferOptions,
                GetTransferHistoryOptions,
                PlaceSpotOrderOptions,
                PlaceMultipleSpotOrdersOptions,
                CancelSpotOrderOptions,
                CancelSpotOrderByClientOrderIdOptions,
                GetOpenSpotOrdersOptions,
                GetClosedSpotOrdersOptions,
                GetSpotUserTradeHistoryOptions,
                GetSpotOrderTradesOptions,
                GetSpotOrderOptions,
                GetSpotOrderByClientOrderIdOptions
                );
        }
    }
}
