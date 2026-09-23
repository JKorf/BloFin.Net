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
        public SubscribePositionOptions SubscribePositionOptions { get; } = new SubscribePositionOptions(_exchangeName, true);
        #region Subscribe To Position Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(SubscribePositionRequest request, Action<DataEvent<SharedPosition[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.SubscribeToPositionUpdatesAsync(
                 update =>
                 {
                     if (update.UpdateType == SocketUpdateType.Snapshot)
                         return;

                     handler(update.ToType(update.Data.Select(x => 
                         new SharedPosition(
                             ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol), 
                             x.Symbol,
                             new SharedOrderQuantity(contractQuantity: Math.Abs(x.PositionSize)), 
                             x.UpdateTime)
                         {
                             AverageOpenPrice = x.AveragePrice,
                             PositionMode = x.PositionSide == PositionSide.Net ? SharedPositionMode.OneWay : SharedPositionMode.HedgeMode,
                             PositionSide = x.PositionSide == Enums.PositionSide.Net ? (x.PositionSize > 0 ? SharedPositionSide.Long : SharedPositionSide.Short) : x.PositionSide == Enums.PositionSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long,
                             UnrealizedPnl = x.UnrealizedPnl,
                             Leverage = x.Leverage,
                             LiquidationPrice = x.LiquidationPrice
                         }).ToArray()));
                 },
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

    }
}
