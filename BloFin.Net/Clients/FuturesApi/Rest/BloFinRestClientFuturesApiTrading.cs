using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class BloFinRestClientFuturesApiTrading : IBloFinRestClientFuturesApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BloFinRestClientFuturesApi _baseClient;
        private readonly ILogger _logger;

        internal BloFinRestClientFuturesApiTrading(ILogger logger, BloFinRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Get Positions

        /// <inheritdoc />
        public async Task<HttpResult<BloFinPosition[]>> GetPositionsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/account/positions", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinPosition[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrderId>> PlaceOrderAsync(string symbol, OrderSide side, OrderType orderType, decimal quantity, MarginMode marginMode, decimal? price = null, PositionSide? positionSide = null, bool? reduceOnly = null, string? clientOrderId = null, decimal? takeProfitTriggerPrice = null, decimal? takeProfitOrderPrice = null, decimal? stopLossTriggerPrice = null, decimal? stopLossOrderPrice = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("side", side);
            parameters.Add("orderType", orderType);
            parameters.Add("size", quantity);
            parameters.Add("marginMode", marginMode);
            parameters.Add("price", price);
            parameters.Add("positionSide", positionSide);
            parameters.Add("reduceOnly", reduceOnly);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("tpTriggerPrice", takeProfitTriggerPrice);
            parameters.Add("tpOrderPrice", takeProfitOrderPrice);
            parameters.Add("slTriggerPrice", stopLossTriggerPrice);
            parameters.Add("slOrderPrice", stopLossOrderPrice);
            parameters.Add("slOrderPrice", stopLossOrderPrice);
            parameters.Add("brokerId", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/trade/order", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrderId[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BloFinOrderId>(result);

            var order = result.Data.FirstOrDefault();
            if (order == null)
                return HttpResult.Fail<BloFinOrderId>(result, new ServerError(ErrorInfo.Unknown with { Message = "No order in response" }));

            if (order.Code != 0)
                return HttpResult.Fail<BloFinOrderId>(result, new ServerError(order.Code, _baseClient.GetErrorInfo(order.Code, order.Message)), order);

            return HttpResult.Ok(result, order);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<HttpResult<CallResult<BloFinOrderId>[]>> PlaceMultipleOrdersAsync(IEnumerable<BloFinOrderRequest> orders, CancellationToken ct = default)
        {
            foreach (var order in orders)
                order.BrokerId = LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange);

            var parameters = new Parameters(orders.ToArray(), BloFinExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/trade/batch-orders", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var response = await _baseClient.SendAsync<BloFinOrderId[]>(request, parameters, ct).ConfigureAwait(false);
            if (!response.Success)
                return HttpResult.Fail<CallResult<BloFinOrderId>[]>(response);

            var result = new List<CallResult<BloFinOrderId>>();
            foreach (var item in response.Data)
            {
                result.Add(item.Code != 0
                    ? CallResult<BloFinOrderId>.Fail(new ServerError(item.Code.ToString(), _baseClient.GetErrorInfo(item.Code, item.Message)))
                    : CallResult<BloFinOrderId>.Ok(item));
            }

            if (result.All(x => !x.Success))
                return HttpResult.Fail(response, new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, false, "All orders failed")), result.ToArray());

            return HttpResult.Ok(response, result.ToArray());
        }

        #endregion

        #region Place Tp Sl Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinTpSlOrderId>> PlaceTpSlOrderAsync(string symbol, OrderSide orderSide, MarginMode marginMode, decimal? quantity = null, PositionSide? positionSide = null, bool? reduceOnly = null, string? clientOrderId = null, decimal? takeProfitTriggerPrice = null, decimal? takeProfitOrderPrice = null, decimal? stopLossTriggerPrice = null, decimal? stopLossOrderPrice = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("side", orderSide);
            parameters.Add("marginMode", marginMode);
            parameters.Add("size", quantity ?? -1);
            parameters.Add("positionSide", positionSide);
            parameters.Add("reduceOnly", reduceOnly);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("tpTriggerPrice", takeProfitTriggerPrice);
            parameters.Add("tpOrderPrice", takeProfitOrderPrice ?? (takeProfitTriggerPrice != null ? -1 : null));
            parameters.Add("slTriggerPrice", stopLossTriggerPrice);
            parameters.Add("slOrderPrice", stopLossOrderPrice ?? (stopLossTriggerPrice != null ? -1 : null));
            parameters.Add("brokerId", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/trade/order-tpsl", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTpSlOrderId>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BloFinTpSlOrderId>(result);

            if (result.Data.Code != 0)
                return HttpResult.Fail(result, new ServerError(result.Data.Code, _baseClient.GetErrorInfo(result.Data.Code, result.Data.Message)), result.Data);

            return HttpResult.Ok(result, result.Data);
        }

        #endregion

        #region Place Trigger Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinAlgoOrderId>> PlaceTriggerOrderAsync(string symbol, OrderSide side, MarginMode marginMode, decimal triggerPrice, TriggerPriceType? triggerPriceType = null, PositionSide? positionSide = null, decimal? quantity = null, string? clientOrderId = null, decimal? orderPrice = null, bool? reduceOnly = null, IEnumerable<BloFinTpSlOrderRequest>? slTpOrders = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("marginMode", marginMode);
            parameters.Add("side", side);
            parameters.Add("triggerPrice", triggerPrice);
            parameters.Add("triggerPriceType", triggerPriceType);
            parameters.Add("positionSide", positionSide);
            parameters.Add("size", quantity ?? -1);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("orderPrice", orderPrice ?? -1);
            parameters.Add("reduceOnly", reduceOnly);
            parameters.Add("orderType", "trigger");
            parameters.Add("brokerId", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            parameters.AddArray("attachAlgoOrders", slTpOrders?.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/trade/order-algo", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrderId>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BloFinAlgoOrderId>(result);

            if (result.Data.Code != 0)
                return HttpResult.Fail(result, new ServerError(result.Data.Code, _baseClient.GetErrorInfo(result.Data.Code, result.Data.Message)), result.Data);

            return HttpResult.Ok(result, result.Data);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrderId>> CancelOrderAsync(string? orderId = null, string? symbol = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/trade/cancel-order", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrderId[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BloFinOrderId>(result);

            var order = result.Data.FirstOrDefault();
            if (order == null)
                return HttpResult.Fail<BloFinOrderId>(result, new ServerError(ErrorInfo.Unknown with { Message = "No order in response" }));

            if (order.Code != 0)
                return HttpResult.Fail(result, new ServerError(order.Code, _baseClient.GetErrorInfo(order.Code, order.Message)), order);

            return HttpResult.Ok(result, order);
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<HttpResult<CallResult<BloFinCancelResponse>[]>> CancelOrdersAsync(IEnumerable<BloFinCancelRequest> orders, CancellationToken ct = default)
        {
            var parameters = new Parameters(orders.ToArray(), BloFinExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/trade/cancel-batch-orders", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var response = await _baseClient.SendAsync<BloFinCancelResponse[]>(request, parameters, ct).ConfigureAwait(false);
            if (!response.Success)
                return HttpResult.Fail<CallResult<BloFinCancelResponse>[]>(response);

            var result = new List<CallResult<BloFinCancelResponse>>();
            foreach (var item in response.Data)
            {
                result.Add(item.Code != 0
                    ? CallResult<BloFinCancelResponse>.Fail(new ServerError(item.Code.ToString(), _baseClient.GetErrorInfo(item.Code, item.Message)))
                    : CallResult<BloFinCancelResponse>.Ok(item));
            }

            if (result.All(x => !x.Success))
                return HttpResult.Fail(response, new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, false, "All cancellations failed")), result.ToArray());

            return HttpResult.Ok(response, result.ToArray());
        }

        #endregion

        #region Cancel Tp Sl Order

        /// <inheritdoc />
        public async Task<HttpResult<CallResult<BloFinTpSlOrderId>[]>> CancelTpSlOrdersAsync(IEnumerable<BloFinCancelTpSlRequest> orders, CancellationToken ct = default)
        {
            var parameters = new Parameters(orders.ToArray(), BloFinExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/trade/cancel-tpsl", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var response = await _baseClient.SendAsync<BloFinTpSlOrderId[]>(request, parameters, ct).ConfigureAwait(false);
            if (!response.Success)
                return HttpResult.Fail<CallResult<BloFinTpSlOrderId>[]>(response);

            var result = new List<CallResult<BloFinTpSlOrderId>>();
            foreach (var item in response.Data)
            {
                result.Add(item.Code != 0
                    ? CallResult<BloFinTpSlOrderId>.Fail(new ServerError(item.Code.ToString(), _baseClient.GetErrorInfo(item.Code, item.Message)))
                    : CallResult<BloFinTpSlOrderId>.Ok(item));
            }

            if (result.All(x => !x.Success))
                return HttpResult.Fail(response, new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, false, "All cancellations failed")), result.ToArray());

            return HttpResult.Ok(response, result.ToArray());
        }

        #endregion

        #region Cancel Trigger Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinAlgoOrderId>> CancelTriggerOrderAsync(string? orderId = null, string? symbol = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("algoId", orderId);
            parameters.Add("instId", symbol);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/trade/cancel-algo", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrderId>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BloFinAlgoOrderId>(result);

            if (result.Data.Code != 0)
                return HttpResult.Fail(result, new ServerError(result.Data.Code, _baseClient.GetErrorInfo(result.Data.Code, result.Data.Message)), result.Data);

            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrder[]>> GetOpenOrdersAsync(string? symbol = null, OrderType? orderType = null, OrderStatus? status = null, string? after = null, string? before = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("orderType", orderType);
            // Before/After filter works inverted on the server
            parameters.Add("after", before);
            parameters.Add("before", after);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/orders-pending", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Tp Sl Orders

        /// <inheritdoc />
        public async Task<HttpResult<BloFinTpSlOrder[]>> GetOpenTpSlOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, string? before = null, string? after = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("tpslId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            // Before/After filter works inverted on the server
            parameters.Add("after", before);
            parameters.Add("before", after);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/orders-tpsl-pending", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTpSlOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Trigger Orders

        /// <inheritdoc />
        public async Task<HttpResult<BloFinAlgoOrder[]>> GetOpenTriggerOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, string? beforeId = null, string? afterId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("algoId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            // Before/After filter works inverted on the server
            parameters.Add("after", beforeId);
            parameters.Add("before", afterId);
            parameters.Add("limit", limit);
            parameters.Add("orderType", "trigger");
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/orders-algo-pending", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Close Position

        /// <inheritdoc />
        public async Task<HttpResult<BloFinClosePositionResult>> ClosePositionAsync(string symbol, MarginMode marginMode, PositionSide? positionSide = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("marginMode", marginMode);
            parameters.Add("positionSide", positionSide);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("brokerId", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/trade/close-position", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinClosePositionResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrder>> GetOrderAsync(string symbol, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/order-detail", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get TpSl Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinTpSlOrder>> GetTpSlOrderAsync(string symbol, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("tpslId", orderId);
            parameters.Add("clientOrderId", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/orders-tpsl-detail", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTpSlOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrder[]>> GetClosedOrdersAsync(string? symbol = null, OrderType? orderType = null, OrderStatus? orderStatus = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("orderType", orderType);
            parameters.Add("state", orderStatus);
            // Before/After filter works inverted on the server
            parameters.Add("before", afterId);
            parameters.Add("after", beforeId);
            parameters.Add("begin", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/orders-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Tp Sl Orders

        /// <inheritdoc />
        public async Task<HttpResult<BloFinTpSlOrder[]>> GetClosedTpSlOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, TpSlOrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("tpslId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("state", status);
            // Before/After filter works inverted on the server
            parameters.Add("after", beforeId);
            parameters.Add("before", afterId);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/orders-tpsl-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTpSlOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Trigger Orders

        /// <inheritdoc />
        public async Task<HttpResult<BloFinAlgoOrder[]>> GetClosedTriggerOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, TpSlOrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("algoId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("state", status);
            // Before/After filter works inverted on the server
            parameters.Add("after", beforeId);
            parameters.Add("before", afterId);
            parameters.Add("limit", limit);
            parameters.Add("orderType", "trigger");
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/orders-algo-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<BloFinUserTrade[]>> GetUserTradesAsync(string? symbol = null, string? orderId = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("orderId", orderId);
            // Before/After filter works inverted on the server
            parameters.Add("after", beforeId);
            parameters.Add("before", afterId);
            parameters.Add("begin", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/fills-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinUserTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Price Limits

        /// <inheritdoc />
        public async Task<HttpResult<BloFinPriceLimit>> GetPriceLimitsAsync(string symbol, OrderSide side, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("side", side);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/order/price-range", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinPriceLimit>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Position History

        /// <inheritdoc />
        public async Task<HttpResult<BloFinPositionHistory[]>> GetPositionHistoryAsync(long? positionId = null, string? symbol = null, PositionStatus? status = null, long? afterId = null, long? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("positionId", positionId);
            parameters.Add("instId", symbol);
            parameters.Add("state", status);
            parameters.Add("after", afterId);
            parameters.Add("before", beforeId);
            parameters.Add("begin", startTime);
            //parameters.Add("end", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/account/positions-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinPositionHistory[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
