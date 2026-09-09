using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BloFin.Net.Clients.FuturesApi
{
    internal partial class BloFinRestClientFuturesSharedApi
    {
        #region Place Futures Trigger Order

        async Task<ICallResult<SharedId>> IPlaceFuturesTriggerOrder.PlaceFuturesTriggerOrderAsync(PlaceFuturesTriggerOrderRequest request, CancellationToken ct)
            => await PlaceFuturesTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public PlaceFuturesTriggerOrderOptions PlaceFuturesTriggerOrderOptions { get; } = new PlaceFuturesTriggerOrderOptions(_exchangeName, false)
        {
            ParameterRuleOverwrites = [
                RequestParameterRuleOverride<PlaceFuturesTriggerOrderRequest>.Required(x => x.PositionMode),
                RequestParameterRuleOverride<PlaceFuturesTriggerOrderRequest>.Required(x => x.MarginMode),
            ]
        };
        public async Task<HttpResult<SharedId>> PlaceFuturesTriggerOrderAsync(PlaceFuturesTriggerOrderRequest request, CancellationToken ct)
        {
            var side = GetTriggerOrderParameters(request.OrderDirection, request.PositionSide);
            var validationError = PlaceFuturesTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.PlaceTriggerOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                side,
                request.MarginMode == SharedMarginMode.Isolated ? MarginMode.Isolated : MarginMode.Cross,
                request.TriggerPrice,
                request.TriggerPriceType == null ? null : request.TriggerPriceType == SharedTriggerPriceType.LastPrice ? TriggerPriceType.LastPrice : request.TriggerPriceType == SharedTriggerPriceType.MarkPrice ? TriggerPriceType.MarkPrice : TriggerPriceType.IndexPrice,
                request.PositionMode == SharedPositionMode.OneWay ? null : request.PositionSide == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,
                request.Quantity.QuantityInContracts,
                orderPrice: request.OrderPrice,
                reduceOnly: request.ReduceOnly == null ? request.OrderDirection == SharedTriggerOrderDirection.Exit : request.ReduceOnly,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            // Return
            return HttpResult.Ok(result, new SharedId(result.Data.AlgoOrderId.ToString()));
        }

        #endregion

        #region Get Futures Trigger Order

        async Task<ICallResult<SharedFuturesTriggerOrder>> IGetFuturesTriggerOrder.GetFuturesTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
            => await GetFuturesTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public GetFuturesTriggerOrderOptions GetFuturesTriggerOrderOptions { get; } = new GetFuturesTriggerOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedFuturesTriggerOrder>> GetFuturesTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTriggerOrder>(Exchange, validationError);

            var order = await _api.Trading.GetOpenTriggerOrdersAsync(orderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedFuturesTriggerOrder>(order);

            if (!order.Data.Any())
            {

                order = await _api.Trading.GetClosedTriggerOrdersAsync(orderId: request.OrderId, ct: ct).ConfigureAwait(false);
                if (!order.Success)
                    return HttpResult.Fail<SharedFuturesTriggerOrder>(order);
            }

            var orderInfo = order.Data.SingleOrDefault();
            if (orderInfo == null)
                return HttpResult.Fail<SharedFuturesTriggerOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            // Return
            return HttpResult.Ok(order, new SharedFuturesTriggerOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, orderInfo.Symbol),
                orderInfo.Symbol,
                orderInfo.OrderId.ToString(),
                orderInfo.TriggerPrice == -1 ? SharedOrderType.Market : SharedOrderType.Limit,
                orderInfo.Side == OrderSide.Buy && orderInfo.PositionSide == PositionSide.Long ? SharedTriggerOrderDirection.Enter : SharedTriggerOrderDirection.Exit,
                ParseTriggerOrderStatus(orderInfo.Status),
                orderInfo.TriggerPrice ?? 0,
                orderInfo.PositionSide == PositionSide.Net ? null : orderInfo.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                orderInfo.CreateTime
                )
            {
                OrderQuantity = new SharedOrderQuantity(contractQuantity: orderInfo.Quantity),
                ClientOrderId = orderInfo.ClientOrderId
            });
        }

        #endregion

        private SharedTriggerOrderStatus ParseTriggerOrderStatus(TpSlOrderStatus status)
        {
            if (status == TpSlOrderStatus.Failed || status == TpSlOrderStatus.Canceled)
                return SharedTriggerOrderStatus.CanceledOrRejected;

            if (status == TpSlOrderStatus.Effective || status == TpSlOrderStatus.Live)
                return SharedTriggerOrderStatus.Active;

            return SharedTriggerOrderStatus.Unknown;
        }

        #region Cancel Futures Trigger Order

        async Task<ICallResult<SharedId>> ICancelFuturesTriggerOrder.CancelFuturesTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesTriggerOrderAsync(request, ct).ConfigureAwait(false);

        public CancelFuturesTriggerOrderOptions CancelFuturesTriggerOrderOptions { get; } = new CancelFuturesTriggerOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelFuturesTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesTriggerOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelTriggerOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.AlgoOrderId.ToString()));
        }

        #endregion

        private OrderSide GetTriggerOrderParameters(SharedTriggerOrderDirection direction, SharedPositionSide side)
        {
            if (direction == SharedTriggerOrderDirection.Enter)
            {
                if (side == SharedPositionSide.Long)
                    // Enter Long = Buy
                    return OrderSide.Buy;
                else
                    // Enter Short = Sell
                    return OrderSide.Sell;
            }

            if (side == SharedPositionSide.Long)
                // Exit Long = Sell
                return OrderSide.Sell;
            else
                // Enter Short = Buy
                return OrderSide.Buy;
        }
    }
}
