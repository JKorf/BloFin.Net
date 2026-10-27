using BloFin.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.SharedApis;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.SpotApi
{
    internal partial class BloFinSocketClientSpotSharedApi :
        SharedApiBase,
        IBloFinSocketClientSpotSharedApi
    {
        private readonly BloFinSocketClientSpotApi _api;

        private const string _topicId = "BloFinSpot";
        private const string _exchangeName = "BloFin";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BloFinExchange.Metadata, this);

        public BloFinSocketClientSpotSharedApi(BloFinSocketClientSpotApi api)
            : base(
                  SharedTransport.Socket,
                  api,
                  new[] { TradingMode.Spot },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeTradeOptions,
                SubscribeTickerOptions,
                SubscribeIncrementalOrderBookOptions,
                SubscribeOrderBookOptions,
                SubscribeKlineOptions,
                SubscribeBalanceOptions,
                SubscribeSpotOrderOptions
                );
        }

        /// <inheritdoc />
        public Task UnsubscribeAllAsync() => _api.UnsubscribeAllAsync();
    }
}
