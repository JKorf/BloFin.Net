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
        public async Task<WebCallResult<BloFinPosition[]>> GetPositionsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/account/positions", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinPosition[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Place Order

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinOrderId>> PlaceOrderAsync(string symbol, OrderSide side, OrderType orderType, decimal quantity, MarginMode marginMode, decimal? price = null, PositionSide? positionSide = null, bool? reduceOnly = null, string? clientOrderId = null, decimal? takeProfitTriggerPrice = null, decimal? takeProfitOrderPrice = null, decimal? stopLossTriggerPrice = null, decimal? stopLossOrderPrice = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("side", side);
            parameters.AddEnum("orderType", orderType);
            parameters.AddString("size", quantity);
            parameters.AddEnum("marginMode", marginMode);
            parameters.AddOptionalString("price", price);
            parameters.AddOptionalEnum("positionSide", positionSide);
            parameters.AddOptional("reduceOnly", reduceOnly);
            parameters.AddOptional("clientOrderId", clientOrderId);
            parameters.AddOptionalString("tpTriggerPrice", takeProfitTriggerPrice);
            parameters.AddOptionalString("tpOrderPrice", takeProfitOrderPrice);
            parameters.AddOptionalString("slTriggerPrice", stopLossTriggerPrice);
            parameters.AddOptionalString("slOrderPrice", stopLossOrderPrice);
            parameters.AddOptionalString("slOrderPrice", stopLossOrderPrice);
            parameters.Add("brokerId", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/trade/order", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrderId[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<BloFinOrderId>(default);

            var order = result.Data.FirstOrDefault();
            if (order == null)
                return result.AsError<BloFinOrderId>(new ServerError(ErrorInfo.Unknown with { Message = "No order in response" }));

            if (order.Code != 0)
                return result.AsErrorWithData(new ServerError(order.Code, _baseClient.GetErrorInfo(order.Code, order.Message)), order);

            return result.As(order);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<WebCallResult<CallResult<BloFinOrderId>[]>> PlaceMultipleOrdersAsync(IEnumerable<BloFinOrderRequest> orders, CancellationToken ct = default)
        {
            foreach (var order in orders)
                order.BrokerId = LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange);

            var parameters = new ParameterCollection();
            parameters.SetBody(orders.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/trade/batch-orders", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var response = await _baseClient.SendAsync<BloFinOrderId[]>(request, parameters, ct).ConfigureAwait(false);
            if (!response.Success)
                return response.As<CallResult<BloFinOrderId>[]>(default);

            var result = new List<CallResult<BloFinOrderId>>();
            foreach (var item in response.Data)
            {
                result.Add(item.Code != 0
                    ? new CallResult<BloFinOrderId>(new ServerError(item.Code.ToString(), _baseClient.GetErrorInfo(item.Code, item.Message)))
                    : new CallResult<BloFinOrderId>(item));
            }

            if (result.All(x => !x.Success))
                return response.AsErrorWithData(new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, false, "All orders failed")), result.ToArray());

            return response.As(result.ToArray());
        }

        #endregion

        #region Place Tp Sl Order

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinTpSlOrderId>> PlaceTpSlOrderAsync(string symbol, OrderSide orderSide, MarginMode marginMode, decimal? quantity = null, PositionSide? positionSide = null, bool? reduceOnly = null, string? clientOrderId = null, decimal? takeProfitTriggerPrice = null, decimal? takeProfitOrderPrice = null, decimal? stopLossTriggerPrice = null, decimal? stopLossOrderPrice = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("side", orderSide);
            parameters.AddEnum("marginMode", marginMode);
            parameters.AddString("size", quantity ?? -1);
            parameters.AddOptionalEnum("positionSide", positionSide);
            parameters.AddOptional("reduceOnly", reduceOnly);
            parameters.AddOptional("clientOrderId", clientOrderId);
            parameters.AddOptionalString("tpTriggerPrice", takeProfitTriggerPrice);
            parameters.AddOptionalString("tpOrderPrice", takeProfitOrderPrice ?? (takeProfitTriggerPrice != null ? -1 : null));
            parameters.AddOptionalString("slTriggerPrice", stopLossTriggerPrice);
            parameters.AddOptionalString("slOrderPrice", stopLossOrderPrice ?? (stopLossTriggerPrice != null ? -1 : null));
            parameters.Add("brokerId", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/trade/order-tpsl", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTpSlOrderId>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<BloFinTpSlOrderId>(default);

            if (result.Data.Code != 0)
                return result.AsErrorWithData(new ServerError(result.Data.Code, _baseClient.GetErrorInfo(result.Data.Code, result.Data.Message)), result.Data);

            return result.As(result.Data);
        }

        #endregion

        #region Place Trigger Order

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinAlgoOrderId>> PlaceTriggerOrderAsync(string symbol, OrderSide side, MarginMode marginMode, decimal triggerPrice, TriggerPriceType? triggerPriceType = null, PositionSide? positionSide = null, decimal? quantity = null, string? clientOrderId = null, decimal? orderPrice = null, bool? reduceOnly = null, IEnumerable<BloFinTpSlOrderRequest>? slTpOrders = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("marginMode", marginMode);
            parameters.AddEnum("side", side);
            parameters.AddString("triggerPrice", triggerPrice);
            parameters.AddOptionalEnum("triggerPriceType", triggerPriceType);
            parameters.AddOptionalEnum("positionSide", positionSide);
            parameters.AddOptionalString("size", quantity ?? -1);
            parameters.AddOptional("clientOrderId", clientOrderId);
            parameters.AddOptionalString("orderPrice", orderPrice ?? -1);
            parameters.AddOptional("reduceOnly", reduceOnly);
            parameters.Add("orderType", "trigger");
            parameters.Add("brokerId", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            parameters.AddOptional("attachAlgoOrders", slTpOrders?.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/trade/order-algo", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrderId>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<BloFinAlgoOrderId>(default);

            if (result.Data.Code != 0)
                return result.AsErrorWithData(new ServerError(result.Data.Code, _baseClient.GetErrorInfo(result.Data.Code, result.Data.Message)), result.Data);

            return result.As(result.Data);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinOrderId>> CancelOrderAsync(string? orderId = null, string? symbol = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instId", symbol);
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/trade/cancel-order", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrderId[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<BloFinOrderId>(default);

            var order = result.Data.FirstOrDefault();
            if (order == null)
                return result.AsError<BloFinOrderId>(new ServerError(ErrorInfo.Unknown with { Message = "No order in response" }));

            if (order.Code != 0)
                return result.AsErrorWithData(new ServerError(order.Code, _baseClient.GetErrorInfo(order.Code, order.Message)), order);

            return result.As(order);
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<WebCallResult<CallResult<BloFinCancelResponse>[]>> CancelOrdersAsync(IEnumerable<BloFinCancelRequest> orders, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(orders.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/trade/cancel-batch-orders", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var response = await _baseClient.SendAsync<BloFinCancelResponse[]>(request, parameters, ct).ConfigureAwait(false);
            if (!response.Success)
                return response.As<CallResult<BloFinCancelResponse>[]>(default);

            var result = new List<CallResult<BloFinCancelResponse>>();
            foreach (var item in response.Data)
            {
                result.Add(item.Code != 0
                    ? new CallResult<BloFinCancelResponse>(new ServerError(item.Code.ToString(), _baseClient.GetErrorInfo(item.Code, item.Message)))
                    : new CallResult<BloFinCancelResponse>(item));
            }

            if (result.All(x => !x.Success))
                return response.AsErrorWithData(new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, false, "All cancellations failed")), result.ToArray());

            return response.As(result.ToArray());
        }

        #endregion

        #region Cancel Tp Sl Order

        /// <inheritdoc />
        public async Task<WebCallResult<CallResult<BloFinTpSlOrderId>[]>> CancelTpSlOrdersAsync(IEnumerable<BloFinCancelTpSlRequest> orders, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.SetBody(orders.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/trade/cancel-tpsl", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var response = await _baseClient.SendAsync<BloFinTpSlOrderId[]>(request, parameters, ct).ConfigureAwait(false);
            if (!response.Success)
                return response.As<CallResult<BloFinTpSlOrderId>[]>(default);

            var result = new List<CallResult<BloFinTpSlOrderId>>();
            foreach (var item in response.Data)
            {
                result.Add(item.Code != 0
                    ? new CallResult<BloFinTpSlOrderId>(new ServerError(item.Code.ToString(), _baseClient.GetErrorInfo(item.Code, item.Message)))
                    : new CallResult<BloFinTpSlOrderId>(item));
            }

            if (result.All(x => !x.Success))
                return response.AsErrorWithData(new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, false, "All cancellations failed")), result.ToArray());

            return response.As(result.ToArray());
        }

        #endregion

        #region Cancel Trigger Order

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinAlgoOrderId>> CancelTriggerOrderAsync(string? orderId = null, string? symbol = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("algoId", orderId);
            parameters.AddOptional("instId", symbol);
            parameters.AddOptional("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/trade/cancel-algo", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrderId>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<BloFinAlgoOrderId>(default);

            if (result.Data.Code != 0)
                return result.AsErrorWithData(new ServerError(result.Data.Code, _baseClient.GetErrorInfo(result.Data.Code, result.Data.Message)), result.Data);

            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinOrder[]>> GetOpenOrdersAsync(string? symbol = null, OrderType? orderType = null, OrderStatus? status = null, string? after = null, string? before = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instId", symbol);
            parameters.AddOptionalEnum("orderType", orderType);
            // Before/After filter works inverted on the server
            parameters.AddOptional("after", before);
            parameters.AddOptional("before", after);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/trade/orders-pending", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Tp Sl Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinTpSlOrder[]>> GetOpenTpSlOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, string? before = null, string? after = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instId", symbol);
            parameters.AddOptional("tpslId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            // Before/After filter works inverted on the server
            parameters.AddOptional("after", before);
            parameters.AddOptional("before", after);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/trade/orders-tpsl-pending", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTpSlOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Trigger Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinAlgoOrder[]>> GetOpenTriggerOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, string? beforeId = null, string? afterId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instId", symbol);
            parameters.AddOptional("algoId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            // Before/After filter works inverted on the server
            parameters.AddOptional("after", beforeId);
            parameters.AddOptional("before", afterId);
            parameters.AddOptional("limit", limit);
            parameters.AddOptional("orderType", "trigger");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/trade/orders-algo-pending", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Close Position

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinClosePositionResult>> ClosePositionAsync(string symbol, MarginMode marginMode, PositionSide? positionSide = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("marginMode", marginMode);
            parameters.AddOptionalEnum("positionSide", positionSide);
            parameters.AddOptional("clientOrderId", clientOrderId);
            parameters.Add("brokerId", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/trade/close-position", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinClosePositionResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinOrder>> GetOrderAsync(string symbol, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddOptional("orderId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/trade/order-detail", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get TpSl Order

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinTpSlOrder>> GetTpSlOrderAsync(string symbol, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddOptional("tpslId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/trade/orders-tpsl-detail", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTpSlOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinOrder[]>> GetClosedOrdersAsync(string? symbol = null, OrderType? orderType = null, OrderStatus? orderStatus = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instId", symbol);
            parameters.AddOptionalEnum("orderType", orderType);
            parameters.AddOptionalEnum("state", orderStatus);
            // Before/After filter works inverted on the server
            parameters.AddOptional("before", afterId);
            parameters.AddOptional("after", beforeId);
            parameters.AddOptionalMillisecondsString("begin", startTime);
            parameters.AddOptionalMillisecondsString("end", endTime);
            parameters.AddOptional("", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/trade/orders-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Tp Sl Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinTpSlOrder[]>> GetClosedTpSlOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, TpSlOrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instId", symbol);
            parameters.AddOptional("tpslId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            parameters.AddOptionalEnum("state", status);
            // Before/After filter works inverted on the server
            parameters.AddOptional("after", beforeId);
            parameters.AddOptional("before", afterId);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/trade/orders-tpsl-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTpSlOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Closed Trigger Orders

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinAlgoOrder[]>> GetClosedTriggerOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, TpSlOrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instId", symbol);
            parameters.AddOptional("algoId", orderId);
            parameters.AddOptional("clientOrderId", clientOrderId);
            parameters.AddOptionalEnum("state", status);
            // Before/After filter works inverted on the server
            parameters.AddOptional("after", beforeId);
            parameters.AddOptional("before", afterId);
            parameters.AddOptional("limit", limit);
            parameters.Add("orderType", "trigger");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/trade/orders-algo-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinUserTrade[]>> GetUserTradesAsync(string? symbol = null, string? orderId = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instId", symbol);
            parameters.AddOptional("orderId", orderId);
            // Before/After filter works inverted on the server
            parameters.AddOptional("after", beforeId);
            parameters.AddOptional("before", afterId);
            parameters.AddOptionalMillisecondsString("begin", startTime);
            parameters.AddOptionalMillisecondsString("end", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/trade/fills-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinUserTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Price Limits

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinPriceLimit>> GetPriceLimitsAsync(string symbol, OrderSide side, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("side", side);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/trade/order/price-range", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinPriceLimit>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
