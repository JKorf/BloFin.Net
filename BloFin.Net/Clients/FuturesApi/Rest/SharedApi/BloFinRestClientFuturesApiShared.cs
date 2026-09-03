using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

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
                  api.Exchange,
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
                GetFuturesTickerOptions,
                GetAllFuturesTickersOptions,
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
                ClosePositionOptions,
                GetFuturesOrderByClientOrderIdOptions,
                CancelFuturesOrderByClientOrderIdOptions,
                SetFuturesTpSlOptions,
                CancelFuturesTpSlOptions,
                PlaceFuturesTriggerOrderOptions,
                GetFuturesTriggerOrderOptions,
                CancelFuturesTriggerOrderOptions,
                GetPositionHistoryOptions
            );
        }
    }
}
