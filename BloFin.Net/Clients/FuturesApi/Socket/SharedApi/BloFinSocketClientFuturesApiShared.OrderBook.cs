using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using CryptoExchange.Net;
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
        public SubscribeOrderBookOptions SubscribeOrderBookOptions { get; } = new SubscribeOrderBookOptions(_exchangeName, false, new[] { 5 })
        {
            SupportsMultipleSymbols = true
        };
        #region Subscribe To Order Book Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(SubscribeOrderBookRequest request, Action<DataEvent<SharedOrderBook>> handler, CancellationToken ct)
        {
            var validationError = SubscribeOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToOrderBookUpdatesAsync(symbols, 5, update => handler(
                update.ToType(
                    new SharedOrderBook(SharedQuantityType.Contracts, update.Data.Sequence, update.Data.Asks, update.Data.Bids))), ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
