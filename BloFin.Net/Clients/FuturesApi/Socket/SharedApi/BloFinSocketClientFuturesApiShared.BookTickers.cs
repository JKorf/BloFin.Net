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
        #region Book Ticker client

        public SubscribeBookTickerOptions SubscribeBookTickerOptions { get; } = new SubscribeBookTickerOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<DataEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToTickerUpdatesAsync(symbols, update => handler(
                update.ToType(
                    new SharedBookTicker(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.BestAskPrice,
                        new SharedOrderQuantity(contractQuantity: update.Data.BestAskQuantity),
                        update.Data.BestBidPrice,
                        new SharedOrderQuantity(contractQuantity: update.Data.BestBidQuantity)))), ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
