using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.SpotApi
{
    internal partial class BloFinSocketClientSpotSharedApi
    {
        #region Subscribe Incremental Order Book

        public SubscribeIncrementalOrderBookOptions SubscribeIncrementalOrderBookOptions { get; } = new SubscribeIncrementalOrderBookOptions(_exchangeName, false, [400], SharedOrderBookSubscriptionType.SnapshotThenIncremental);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(SubscribeOrderBookRequest request, Action<DataEvent<SharedIncrementalOrderBook>> handler, CancellationToken ct)
        {
            var validationError = SubscribeIncrementalOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.SubscribeToOrderBookUpdatesAsync(
                symbol,
                400,
                update => handler(update.ToType(
                    new SharedIncrementalOrderBook(
                        SharedQuantityType.BaseAsset,
                        update.Data.PrevSequence > 0 ? update.Data.PrevSequence + 1 : null,
                        update.Data.Sequence,
                        update.Data.Asks,
                        update.Data.Bids
                        )
                    {
                    })), ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Subscribe Order Book

        public SubscribeOrderBookOptions SubscribeOrderBookOptions { get; } = new SubscribeOrderBookOptions(_exchangeName, false, [5]);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(SubscribeOrderBookRequest request, Action<DataEvent<SharedOrderBook>> handler, CancellationToken ct)
        {
            var validationError = SubscribeOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.SubscribeToOrderBookUpdatesAsync(
                symbol,
                5,
                update => handler(update.ToType(
                    new SharedOrderBook(
                        SharedQuantityType.BaseAsset,
                        update.Data.Sequence,
                        update.Data.Asks,
                        update.Data.Bids
                        )
                    {
                    })), ct).ConfigureAwait(false);

            return result;
        }

        #endregion

    }
}
