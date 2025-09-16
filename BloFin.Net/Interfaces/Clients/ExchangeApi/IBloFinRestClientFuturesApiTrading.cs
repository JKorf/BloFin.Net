using BloFin.Net.Enums;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// BloFin Exchange trading endpoints, placing and managing orders.
    /// </summary>
    public interface IBloFinRestClientFuturesApiTrading
    {
        /// <summary>
        /// Get positions
        /// <para><a href="https://docs.blofin.com/index.html#get-positions" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinPosition[]>> GetPositionsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para><a href="https://docs.blofin.com/index.html#place-order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="side">Order side</param>
        /// <param name="orderType">Order type</param>
        /// <param name="quantity">Quantity in contracts</param>
        /// <param name="marginMode">Margin mode</param>
        /// <param name="price">Limit price</param>
        /// <param name="positionSide">Position side</param>
        /// <param name="reduceOnly">Reduce only order</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="takeProfitTriggerPrice">Take profit trigger price</param>
        /// <param name="takeProfitOrderPrice">Take profit order price</param>
        /// <param name="stopLossTriggerPrice">Stop loss trigger price</param>
        /// <param name="stopLossOrderPrice">Stop loss order price</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinOrderId>> PlaceOrderAsync(string symbol, OrderSide side, OrderType orderType, decimal quantity, MarginMode marginMode, decimal? price = null, PositionSide? positionSide = null, bool? reduceOnly = null, string? clientOrderId = null, decimal? takeProfitTriggerPrice = null, decimal? takeProfitOrderPrice = null, decimal? stopLossTriggerPrice = null, decimal? stopLossOrderPrice = null, CancellationToken ct = default);

        /// <summary>
        /// Place multiple new orders in a single request
        /// <para><a href="https://docs.blofin.com/index.html#place-multiple-orders" /></para>
        /// </summary>
        /// <param name="orders">Orders to place</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CallResult<BloFinOrderId>[]>> PlaceMultipleOrdersAsync(IEnumerable<BloFinOrderRequest> orders, CancellationToken ct = default);

        /// <summary>
        /// Place a new TP/SL order
        /// <para><a href="https://docs.blofin.com/index.html#place-tpsl-order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="orderSide">Order side</param>
        /// <param name="marginMode">Margin mode</param>
        /// <param name="quantity">Order quantity, if not send uses whole position</param>
        /// <param name="positionSide">Position side</param>
        /// <param name="reduceOnly">Reduce only</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="takeProfitTriggerPrice">Take profit trigger price</param>
        /// <param name="takeProfitOrderPrice">Take profit order price</param>
        /// <param name="stopLossTriggerPrice">Stop loss trigger price</param>
        /// <param name="stopLossOrderPrice">Stop loss order price</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTpSlOrderId>> PlaceTpSlOrderAsync(string symbol, OrderSide orderSide, MarginMode marginMode, decimal? quantity = null, PositionSide? positionSide = null, bool? reduceOnly = null, string? clientOrderId = null, decimal? takeProfitTriggerPrice = null, decimal? takeProfitOrderPrice = null, decimal? stopLossTriggerPrice = null, decimal? stopLossOrderPrice = null, CancellationToken ct = default);

        /// <summary>
        /// Place new trigger order
        /// <para><a href="https://docs.blofin.com/index.html#place-algo-order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="marginMode">Margin mode</param>
        /// <param name="side">Order side</param>
        /// <param name="triggerPrice">Trigger price</param>
        /// <param name="triggerPriceType">Trigger price type</param>
        /// <param name="positionSide">Position side</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="orderPrice">Order price</param>
        /// <param name="reduceOnly">Reduce only</param>
        /// <param name="slTpOrders">Stop loss / take profit orders</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinAlgoOrderId>> PlaceTriggerOrderAsync(string symbol, OrderSide side, MarginMode marginMode, decimal triggerPrice, TriggerPriceType? triggerPriceType = null, PositionSide? positionSide = null, decimal? quantity = null, string? clientOrderId = null, decimal? orderPrice = null, bool? reduceOnly = null, IEnumerable<BloFinTpSlOrderRequest>? slTpOrders = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an open order
        /// <para><a href="https://docs.blofin.com/index.html#cancel-order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`. Only required when using clientOrderId</param>
        /// <param name="orderId">The order id. Either this or `clientOrderId` should be provided</param>
        /// <param name="clientOrderId">The client order id. Either this or `orderId` should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinOrderId>> CancelOrderAsync(string? orderId = null, string? symbol = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple orders
        /// <para><a href="https://docs.blofin.com/index.html#cancel-multiple-orders" /></para>
        /// </summary>
        /// <param name="orders">Orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CallResult<BloFinCancelResponse>[]>> CancelOrdersAsync(IEnumerable<BloFinCancelRequest> orders, CancellationToken ct = default);

        /// <summary>
        /// Cancel TpSl orders
        /// <para><a href="https://docs.blofin.com/index.html#cancel-tpsl-order" /></para>
        /// </summary>
        /// <param name="orders">Orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CallResult<BloFinTpSlOrderId>[]>> CancelTpSlOrdersAsync(IEnumerable<BloFinCancelTpSlRequest> orders, CancellationToken ct = default);

        /// <summary>
        /// Cancel a trigger order
        /// <para><a href="https://docs.blofin.com/index.html#cancel-algo-order" /></para>
        /// </summary>
        /// <param name="orderId">Algo order id</param>
        /// <param name="symbol">Symbol name, for example `ETH-USDT`</param>
        /// <param name="clientOrderId">Client order id, either this or `orderId` should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinAlgoOrderId>> CancelTriggerOrderAsync(string? orderId = null, string? symbol = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para><a href="https://docs.blofin.com/index.html#get-active-orders" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderType">Filter by order type</param>
        /// <param name="status">Filter by order status</param>
        /// <param name="afterId">Return results after this id</param>
        /// <param name="beforeId">Return results before this id</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinOrder[]>> GetOpenOrdersAsync(string? symbol = null, OrderType? orderType = null, OrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get open TP/SL orders
        /// <para><a href="https://docs.blofin.com/index.html#get-active-tpsl-orders" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Filter by tpsl order id</param>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="afterId">Return results after this id</param>
        /// <param name="beforeId">Return results before this id</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTpSlOrder[]>> GetOpenTpSlOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get open trigger orders
        /// <para><a href="https://docs.blofin.com/index.html#get-active-algo-orders" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="orderId">Filter by algo order id</param>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="beforeId">Return results before this id</param>
        /// <param name="afterId">Return results after this id</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinAlgoOrder[]>> GetOpenTriggerOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, string? beforeId = null, string? afterId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Close a position at market price
        /// <para><a href="https://docs.blofin.com/index.html#close-positions" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="marginMode">Margin mode</param>
        /// <param name="positionSide">Position side</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinClosePositionResult>> ClosePositionAsync(string symbol, MarginMode marginMode, PositionSide? positionSide = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get closed order history
        /// <para><a href="https://docs.blofin.com/index.html#get-order-history" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderType">Filter by order type</param>
        /// <param name="orderStatus">Filter by order status</param>
        /// <param name="afterId">Return results after this id</param>
        /// <param name="beforeId">Return results before this id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinOrder[]>> GetClosedOrdersAsync(string? symbol = null, OrderType? orderType = null, OrderStatus? orderStatus = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get closed tp sl order history
        /// <para><a href="https://docs.blofin.com/index.html#get-tpsl-order-history" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
        /// <param name="orderId">Filter by tpsl order id</param>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="status">Filter by status</param>
        /// <param name="afterId">Return results after this id</param>
        /// <param name="beforeId">Return results before this id</param>
        /// <param name="limit">Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTpSlOrder[]>> GetClosedTpSlOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, TpSlOrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get closed trigger order history
        /// <para><a href="https://docs.blofin.com/index.html#get-algo-order-history" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Filter by algo order id</param>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="status">Filter by status</param>
        /// <param name="afterId">Return results after this id</param>
        /// <param name="beforeId">Return results before this id</param>
        /// <param name="limit">Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinAlgoOrder[]>> GetClosedTriggerOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, TpSlOrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get user trade history
        /// <para><a href="https://docs.blofin.com/index.html#get-trade-history" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="afterId">Return results after this id</param>
        /// <param name="beforeId">Return results before this id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinUserTrade[]>> GetUserTradesAsync(string? symbol = null, string? orderId = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get order price limits
        /// <para><a href="https://docs.blofin.com/index.html#get-trade-order-price-range" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="side">Order side</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinPriceLimit>> GetPriceLimitsAsync(string symbol, OrderSide side, CancellationToken ct = default);

    }
}
