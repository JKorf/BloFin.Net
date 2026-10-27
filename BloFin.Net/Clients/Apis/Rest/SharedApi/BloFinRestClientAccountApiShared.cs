using BloFin.Net.Interfaces.Clients.AccountApi;
using CryptoExchange.Net.SharedApis;

namespace BloFin.Net.Clients.Apis
{
    internal partial class BloFinRestClientAccountSharedApi : 
        SharedApiBase,
        IBloFinRestClientAccountApiShared,
        IBloFinRestClientAccountSharedApi
    {
        private readonly BloFinRestClientAccountApi _api;

        private const string _exchangeName = "BloFin";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BloFinExchange.Metadata, this);

        public BloFinRestClientAccountSharedApi(BloFinRestClientAccountApi api)
            : base(
                  SharedTransport.Rest,
                  api,
                  new[] { TradingMode.Spot },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetWithdrawalHistoryOptions,
                GetDepositHistoryOptions
            );
        }
    }
}
