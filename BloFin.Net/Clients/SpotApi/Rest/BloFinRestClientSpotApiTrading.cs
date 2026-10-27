using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.SpotApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class BloFinRestClientSpotApiTrading : IBloFinRestClientSpotApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BloFinRestClientSpotApi _baseClient;

        /// <summary>
        /// ctor
        /// </summary>
        internal BloFinRestClientSpotApiTrading(BloFinRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrderId>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal quantity,
            decimal? price = null,
            AssetType? assetType = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("side", side);
            parameters.Add("orderType", type);
            parameters.Add("size", quantity);
            parameters.Add("price", price);
            parameters.Add("targetCurrency", assetType);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("brokerId", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/spot/trade/order", BloFinExchange.RateLimiter.BloFinRest, 1, true);
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
        public async Task<HttpResult<CallResult<BloFinOrderId>[]>> PlaceMultipleOrdersAsync(IEnumerable<BloFinSpotOrderRequest> orders, CancellationToken ct = default)
        {
            var intOrders = orders.ToArray();
            foreach (var order in intOrders)
                order.BrokerId = LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange);

            var parameters = new Parameters(intOrders, BloFinExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/spot/trade/batch-orders", BloFinExchange.RateLimiter.BloFinRest, 1, true);
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

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrderId>> CancelOrderAsync(
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/spot/trade/cancel-order", BloFinExchange.RateLimiter.BloFinRest, 1, true);
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

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrder[]>> GetOpenOrdersAsync(
            string? symbol = null,
            OrderStatus? status = null,
            string? startId = null,
            string? endId = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("state", status);
            parameters.Add("after", startId);
            parameters.Add("before", endId);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/trade/orders-pending", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrder>> GetOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/trade/order-detail", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order History

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrder[]>> GetOrderHistoryAsync(
            string? symbol = null,
            OrderStatus? status = null,
            string? startId = null,
            string? endId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("state", status);
            parameters.Add("after", startId);
            parameters.Add("before", endId);
            parameters.Add("begin", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/trade/orders-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Place Algo Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinAlgoOrderId>> PlaceAlgoOrderAsync(
            string symbol,
            OrderSide side,
            AlgoOrderType type,
            decimal quantity,
            decimal triggerPrice,
            string? clientOrderId = null,
            decimal? price = null,
            TriggerPriceType? triggerPriceType = null,
            AssetType? targetAsset = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("side", side);
            parameters.Add("orderType", type);
            parameters.Add("size", quantity);
            parameters.Add("triggerPrice", triggerPrice);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("orderPrice", price ?? -1);
            parameters.Add("triggerPriceType", triggerPriceType);
            parameters.Add("targetCurrency", targetAsset);
            parameters.Add("brokerId", LibraryHelpers.GetClientReference(() => _baseClient.ClientOptions.BrokerId, _baseClient.Exchange));
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/spot/trade/order-algo", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrderId>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Algo Order

        /// <inheritdoc />
        public async Task<HttpResult<BloFinAlgoOrderId>> CancelAlgoOrderAsync(
            string symbol,
            string? algoOrderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("algoId", algoOrderId);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/spot/trade/cancel-algo", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrderId>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Algo Orders

        /// <inheritdoc />
        public async Task<HttpResult<BloFinAlgoOrder[]>> GetOpenAlgoOrdersAsync(
            string? symbol = null,
            string? orderId = null,
            string? clientOrderId = null,
            string? startId = null,
            string? endId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("algoId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("after", startId);
            parameters.Add("before", endId);
            parameters.Add("limit", limit);
            parameters.Add("orderType", "trigger");
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/trade/orders-algo-pending", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Algo Order History

        /// <inheritdoc />
        public async Task<HttpResult<BloFinAlgoOrder[]>> GetAlgoOrderHistoryAsync(
            string? symbol = null,
            string? orderId = null,
            string? clientOrderId = null,
            OrderStatus? status = null,
            string? startId = null,
            string? endId = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("algoId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("state", status);
            // Before/After filter works inverted on the server
            parameters.Add("after", startId);
            parameters.Add("before", endId);
            parameters.Add("limit", limit);
            parameters.Add("orderType", "trigger");
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/trade/orders-algo-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinAlgoOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<BloFinUserTrade[]>> GetUserTradesAsync(
            string? symbol = null,
            string? orderId = null,
            string? startId = null,
            string? endId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("after", startId);
            parameters.Add("before", endId);
            parameters.Add("begin", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/trade/fills-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinUserTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
