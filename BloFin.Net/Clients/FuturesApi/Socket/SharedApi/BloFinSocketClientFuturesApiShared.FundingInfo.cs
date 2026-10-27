using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.FuturesApi
{
    internal partial class BloFinSocketClientFuturesSharedApi
    {
        #region Subscribe Funding Info

        public SubscribeFundingInfoOptions SubscribeFundingInfoOptions { get; } = new SubscribeFundingInfoOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToFundingInfoUpdatesAsync(SubscribeFundingInfoRequest request, Action<DataEvent<SharedFundingInfo>> handler, CancellationToken ct)
        {
            var validationError = SubscribeFundingInfoOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.SubscribeToFundingRateUpdatesAsync(
                symbol,
                update => handler(update.ToType<SharedFundingInfo>(

                    new SharedFundingInfo(
                        update.Data.FundingRate,
                        update.Data.FundingTime,
                        null
                        )

                )
            )
            , ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
