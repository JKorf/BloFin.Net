using BloFin.Net.Interfaces.Clients.FuturesApi;
using CryptoExchange.Net.SharedApis;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.FuturesApi
{
    internal partial class BloFinSocketClientFuturesSharedApi :
        SharedApiBase,
        IBloFinSocketClientFuturesApiShared,
        IBloFinSocketClientFuturesSharedApi
    {
        private readonly BloFinSocketClientFuturesApi _api;

        private const string _topicId = "BloFinFutures";
        private const string _exchangeName = "BloFin";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BloFinExchange.Metadata, this);

        public BloFinSocketClientFuturesSharedApi(BloFinSocketClientFuturesApi api)
            : base(
                  SharedTransport.Socket,
                  api,
                  new[] { TradingMode.PerpetualLinear, TradingMode.PerpetualInverse },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeBookTickerOptions,
                SubscribeKlineOptions,
                SubscribeOrderBookOptions,
                SubscribeTickerOptions,
                SubscribeTradeOptions,
                SubscribeBalanceOptions,
                SubscribeFuturesOrderOptions,
                SubscribePositionOptions,
                SubscribeFundingInfoOptions
            );
        }

        /// <inheritdoc />
        public Task UnsubscribeAllAsync() => _api.UnsubscribeAllAsync();
    }
}
