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
        #region Kline client
        public SubscribeKlineOptions SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {
            var validationError = SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var interval = (Enums.KlineInterval)request.Interval;
            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToKlineUpdatesAsync(symbols, interval, update => handler(update.ToType(
                new SharedKline(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Symbol),
                    update.Symbol!,
                    update.Data.OpenTime, 
                    update.Data.ClosePrice, 
                    update.Data.HighPrice, 
                    update.Data.LowPrice, 
                    update.Data.OpenPrice,
                    new SharedOrderQuantity(update.Data.BaseVolume, update.Data.QuoteVolume, update.Data.Volume))
                {
#pragma warning disable CS0618 // Type or member is obsolete | Temporary to maintain previous behavior
                    Volume = update.Data.Volume
#pragma warning restore
                })), ct).ConfigureAwait(false);

            return result;
        }
        #endregion
    }
}
