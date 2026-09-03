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
        #region Futures Order client
        async Task<WebSocketResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
            => await SubscribeToFuturesOrderUpdatesAsync(request, x => handler(x.ToType<SharedFuturesOrder[]>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeFuturesOrderOptions SubscribeFuturesOrderOptions { get; } = new SubscribeFuturesOrderOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.SubscribeToOrderUpdatesAsync(
                update =>
                {
                    if (update.UpdateType == SocketUpdateType.Snapshot)
                        return;

                    handler(update.ToType(update.Data.Select(x =>
                        new SharedFuturesOrderUpdate(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol), x.Symbol,
                            x.OrderId.ToString(),
                            ParseOrderType(x.OrderType),
                            x.Side == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseOrderStatus(x.Status),
                            x.UpdateTime)
                        {
                            ClientOrderId = x.ClientOrderId,
                            OrderPrice = x.Price == 0 ? null : x.Price,
                            OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                            QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                            UpdateTime = x.UpdateTime,
#pragma warning disable CS0618 // Type or member is obsolete
                            Fee = x.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                            AveragePrice = x.AveragePrice == 0 ? null : x.AveragePrice,
                            PositionSide = x.PositionSide == Enums.PositionSide.Long ? SharedPositionSide.Long : x.PositionSide == Enums.PositionSide.Short ? SharedPositionSide.Short : null,
                            ReduceOnly = x.ReduceOnly,
                            TimeInForce = ParseTimeInForce(x.OrderType),
                            StopLossPrice = x.StopLossTriggerPrice,
                            TakeProfitPrice = x.TakeProfitTriggerPrice,
                            Leverage = x.Leverage,
                        }).ToArray()
                    ));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.Open || status == OrderStatus.PartiallyFilled) return SharedOrderStatus.Open;
            if (status == OrderStatus.Filled) return SharedOrderStatus.Filled;
            if (status == OrderStatus.Canceled || status == OrderStatus.PartiallyCanceled) return SharedOrderStatus.Canceled;

            return SharedOrderStatus.Unknown;
        }

        private SharedOrderType ParseOrderType(OrderType type)
        {
            if (type == OrderType.Market || type == OrderType.ImmediateOrCancel)
                return SharedOrderType.Market;

            if (type == OrderType.Limit || type == OrderType.PostOnly)
                return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

        private SharedTimeInForce? ParseTimeInForce(OrderType type)
        {
            if (type == OrderType.PostOnly || type == OrderType.Limit) return SharedTimeInForce.GoodTillCanceled;
            if (type == OrderType.FillOrKill) return SharedTimeInForce.FillOrKill;
            if (type == OrderType.ImmediateOrCancel || type == OrderType.Market) return SharedTimeInForce.ImmediateOrCancel;

            return null;
        }
        #endregion
    }
}
