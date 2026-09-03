using BloFin.Net.Clients.FuturesApi;
using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.AccountApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                  api.Exchange,
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
