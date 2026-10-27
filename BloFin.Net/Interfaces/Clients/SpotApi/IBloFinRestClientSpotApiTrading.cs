using BloFin.Net.Enums;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BloFin Spot Trading endpoints
    /// </summary>
    public interface IBloFinRestClientSpotApiTrading
    {
        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#place-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/spot/trade/order<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="side">["<c>side</c>"] Order side</param>
        /// <param name="type">["<c>orderType</c>"] Order type</param>
        /// <param name="quantity">["<c>size</c>"] Quantity</param>
        /// <param name="price">["<c>price</c>"] Limit price</param>
        /// <param name="assetType">["<c>targetCurrency</c>"] Target asset for market orders</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinOrderId>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal quantity,
            decimal? price = null,
            AssetType? assetType = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Place multiple new orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#place-multiple-orders" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/spot/trade/batch-orders<br />
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to place</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<CallResult<BloFinOrderId>[]>> PlaceMultipleOrdersAsync(IEnumerable<BloFinSpotOrderRequest> orders, CancellationToken ct = default);

        /// <summary>
        /// Cancel an active order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#cancel-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/spot/trade/cancel-order<br />
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>orderId</c>"] Order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id, either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinOrderId>> CancelOrderAsync(
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get open orders list
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-active-orders" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/spot/trade/orders-pending<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETHUSDT`</param>
        /// <param name="status">["<c>state</c>"] Filter by status</param>
        /// <param name="startId">["<c>after</c>"] Filter by start id</param>
        /// <param name="endId">["<c>before</c>"] Filter by end id</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinOrder[]>> GetOpenOrdersAsync(
            string? symbol = null,
            OrderStatus? status = null,
            string? startId = null,
            string? endId = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get order by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-order-detail" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/spot/trade/order-detail<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>orderId</c>"] Order id. Either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id, either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinOrder>> GetOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get order history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-order-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/spot/trade/orders-history<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="status">["<c>state</c>"] Filter by status</param>
        /// <param name="startId">["<c>begin</c>"] Filter by start id</param>
        /// <param name="endId">["<c>end</c>"] Filter by end id</param>
        /// <param name="startTime">["<c>before</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>after</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinOrder[]>> GetOrderHistoryAsync(
            string? symbol = null,
            OrderStatus? status = null,
            string? startId = null,
            string? endId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Place a new algo order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#place-algo-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/spot/trade/order-algo<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETHUSDT`</param>
        /// <param name="side">["<c>side</c>"] Order side</param>
        /// <param name="type">["<c>orderType</c>"] Algo order type</param>
        /// <param name="quantity">["<c>size</c>"] Quantity</param>
        /// <param name="triggerPrice">["<c>triggerPrice</c>"] Trigger price</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id</param>
        /// <param name="price">["<c>orderPrice</c>"] Trigger order price</param>
        /// <param name="triggerPriceType">["<c>triggerPriceType</c>"] Price type</param>
        /// <param name="targetAsset">["<c>targetCurrency</c>"] Quantity asset type for market orders</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinAlgoOrderId>> PlaceAlgoOrderAsync(
            string symbol,
            OrderSide side,
            AlgoOrderType type,
            decimal quantity,
            decimal triggerPrice,
            string? clientOrderId = null,
            decimal? price = null,
            TriggerPriceType? triggerPriceType = null,
            AssetType? targetAsset = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an active algo order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#cancel-algo-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/spot/trade/cancel-algo<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETHUSDT`</param>
        /// <param name="algoOrderId">["<c>algoId</c>"] Algo order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id, either this or algoOrderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinAlgoOrderId>> CancelAlgoOrderAsync(
            string symbol,
            string? algoOrderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get open algo orders list
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-active-algo-orders" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/spot/trade/orders-algo-pending<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>orderId</c>"] Filter by algo id</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Filter by client order id</param>
        /// <param name="startId">["<c>startId</c>"] Filter by start id</param>
        /// <param name="endId">["<c>endId</c>"] Filter by end id</param>
        /// <param name="startTime">["<c>after</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>before</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinAlgoOrder[]>> GetOpenAlgoOrdersAsync(
            string? symbol = null,
            string? orderId = null,
            string? clientOrderId = null,
            string? startId = null,
            string? endId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get algo order history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-algo-order-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/spot/trade/orders-algo-history<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETHUSDT`</param>
        /// <param name="orderId">["<c>algoId</c>"] Filter by algo order id</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Filter by client order id</param>
        /// <param name="status">["<c>state</c>"] Filter by status</param>
        /// <param name="startId">["<c>after</c>"] Filter by start id</param>
        /// <param name="endId">["<c>before</c>"] Filter by end id</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinAlgoOrder[]>> GetAlgoOrderHistoryAsync(
            string? symbol = null,
            string? orderId = null,
            string? clientOrderId = null,
            OrderStatus? status = null,
            string? startId = null,
            string? endId = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get user trade history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-trade-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/spot/trade/fills-history<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>orderId</c>"] Filter by order id</param>
        /// <param name="startId">["<c>begin</c>"] Filter by start id</param>
        /// <param name="endId">["<c>end</c>"] Filter by end id</param>
        /// <param name="startTime">["<c>after</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>before</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinUserTrade[]>> GetUserTradesAsync(
            string? symbol = null,
            string? orderId = null,
            string? startId = null,
            string? endId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

    }
}
