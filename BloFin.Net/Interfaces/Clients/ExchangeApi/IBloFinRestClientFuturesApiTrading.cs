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
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-positions" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/account/positions
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinPosition[]>> GetPositionsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#place-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/trade/order
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="side">["<c>side</c>"] Order side</param>
        /// <param name="orderType">["<c>orderType</c>"] Order type</param>
        /// <param name="quantity">["<c>size</c>"] Quantity in contracts</param>
        /// <param name="marginMode">["<c>marginMode</c>"] Margin mode</param>
        /// <param name="price">["<c>price</c>"] Limit price</param>
        /// <param name="positionSide">["<c>positionSide</c>"] Position side</param>
        /// <param name="reduceOnly">["<c>reduceOnly</c>"] Reduce only order</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id</param>
        /// <param name="takeProfitTriggerPrice">["<c>tpTriggerPrice</c>"] Take profit trigger price</param>
        /// <param name="takeProfitOrderPrice">["<c>tpOrderPrice</c>"] Take profit order price</param>
        /// <param name="stopLossTriggerPrice">["<c>slTriggerPrice</c>"] Stop loss trigger price</param>
        /// <param name="stopLossOrderPrice">["<c>slOrderPrice</c>"] Stop loss order price</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinOrderId>> PlaceOrderAsync(string symbol, OrderSide side, OrderType orderType, decimal quantity, MarginMode marginMode, decimal? price = null, PositionSide? positionSide = null, bool? reduceOnly = null, string? clientOrderId = null, decimal? takeProfitTriggerPrice = null, decimal? takeProfitOrderPrice = null, decimal? stopLossTriggerPrice = null, decimal? stopLossOrderPrice = null, CancellationToken ct = default);

        /// <summary>
        /// Place multiple new orders in a single request
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#place-multiple-orders" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/trade/batch-orders
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to place</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CallResult<BloFinOrderId>[]>> PlaceMultipleOrdersAsync(IEnumerable<BloFinOrderRequest> orders, CancellationToken ct = default);

        /// <summary>
        /// Place a new TP/SL order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#place-tpsl-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/trade/order-tpsl
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="orderSide">["<c>side</c>"] Order side</param>
        /// <param name="marginMode">["<c>marginMode</c>"] Margin mode</param>
        /// <param name="quantity">["<c>size</c>"] Order quantity, if not send uses whole position</param>
        /// <param name="positionSide">["<c>positionSide</c>"] Position side</param>
        /// <param name="reduceOnly">["<c>reduceOnly</c>"] Reduce only</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id</param>
        /// <param name="takeProfitTriggerPrice">["<c>tpTriggerPrice</c>"] Take profit trigger price</param>
        /// <param name="takeProfitOrderPrice">["<c>tpOrderPrice</c>"] Take profit order price</param>
        /// <param name="stopLossTriggerPrice">["<c>slTriggerPrice</c>"] Stop loss trigger price</param>
        /// <param name="stopLossOrderPrice">["<c>slOrderPrice</c>"] Stop loss order price</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTpSlOrderId>> PlaceTpSlOrderAsync(string symbol, OrderSide orderSide, MarginMode marginMode, decimal? quantity = null, PositionSide? positionSide = null, bool? reduceOnly = null, string? clientOrderId = null, decimal? takeProfitTriggerPrice = null, decimal? takeProfitOrderPrice = null, decimal? stopLossTriggerPrice = null, decimal? stopLossOrderPrice = null, CancellationToken ct = default);

        /// <summary>
        /// Place new trigger order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#place-algo-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/trade/order-algo
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="marginMode">["<c>marginMode</c>"] Margin mode</param>
        /// <param name="side">["<c>side</c>"] Order side</param>
        /// <param name="triggerPrice">["<c>triggerPrice</c>"] Trigger price</param>
        /// <param name="triggerPriceType">["<c>triggerPriceType</c>"] Trigger price type</param>
        /// <param name="positionSide">["<c>positionSide</c>"] Position side</param>
        /// <param name="quantity">["<c>size</c>"] Quantity</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id</param>
        /// <param name="orderPrice">["<c>orderPrice</c>"] Order price</param>
        /// <param name="reduceOnly">["<c>reduceOnly</c>"] Reduce only</param>
        /// <param name="slTpOrders">["<c>attachAlgoOrders</c>"] Stop loss / take profit orders</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinAlgoOrderId>> PlaceTriggerOrderAsync(string symbol, OrderSide side, MarginMode marginMode, decimal triggerPrice, TriggerPriceType? triggerPriceType = null, PositionSide? positionSide = null, decimal? quantity = null, string? clientOrderId = null, decimal? orderPrice = null, bool? reduceOnly = null, IEnumerable<BloFinTpSlOrderRequest>? slTpOrders = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an open order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#cancel-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/trade/cancel-order
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`. Only required when using clientOrderId</param>
        /// <param name="orderId">["<c>orderId</c>"] The order id. Either this or `clientOrderId` should be provided</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] The client order id. Either this or `orderId` should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinOrderId>> CancelOrderAsync(string? orderId = null, string? symbol = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#cancel-multiple-orders" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/trade/cancel-batch-orders
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CallResult<BloFinCancelResponse>[]>> CancelOrdersAsync(IEnumerable<BloFinCancelRequest> orders, CancellationToken ct = default);

        /// <summary>
        /// Cancel TpSl orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#cancel-tpsl-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/trade/cancel-tpsl
        /// </para>
        /// </summary>
        /// <param name="orders">Orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<CallResult<BloFinTpSlOrderId>[]>> CancelTpSlOrdersAsync(IEnumerable<BloFinCancelTpSlRequest> orders, CancellationToken ct = default);

        /// <summary>
        /// Cancel a trigger order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#cancel-algo-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/trade/cancel-algo
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>algoId</c>"] Algo order id</param>
        /// <param name="symbol">["<c>instId</c>"] Symbol name, for example `ETH-USDT`</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id, either this or `orderId` should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinAlgoOrderId>> CancelTriggerOrderAsync(string? orderId = null, string? symbol = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-active-orders" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/orders-pending
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderType">["<c>orderType</c>"] Filter by order type</param>
        /// <param name="status">Filter by order status</param>
        /// <param name="afterId">["<c>before</c>"] Return results after this id</param>
        /// <param name="beforeId">["<c>after</c>"] Return results before this id</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinOrder[]>> GetOpenOrdersAsync(string? symbol = null, OrderType? orderType = null, OrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get open TP/SL orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-active-tpsl-orders" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/orders-tpsl-pending
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>tpslId</c>"] Filter by tpsl order id</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Filter by client order id</param>
        /// <param name="afterId">["<c>before</c>"] Return results after this id</param>
        /// <param name="beforeId">["<c>after</c>"] Return results before this id</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTpSlOrder[]>> GetOpenTpSlOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get open trigger orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-active-algo-orders" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/orders-algo-pending
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETHUSDT`</param>
        /// <param name="orderId">["<c>algoId</c>"] Filter by algo order id</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Filter by client order id</param>
        /// <param name="beforeId">["<c>after</c>"] Return results before this id</param>
        /// <param name="afterId">["<c>before</c>"] Return results after this id</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinAlgoOrder[]>> GetOpenTriggerOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, string? beforeId = null, string? afterId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Close a position at market price
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#close-positions" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/trade/close-position
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="marginMode">["<c>marginMode</c>"] Margin mode</param>
        /// <param name="positionSide">["<c>positionSide</c>"] Position side</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinClosePositionResult>> ClosePositionAsync(string symbol, MarginMode marginMode, PositionSide? positionSide = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get details of a specific order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-order-detail" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/order-detail
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>orderId</c>"] The order id. Either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] The client order id. Either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinOrder>> GetOrderAsync(string symbol, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get details of a specific Tp/Sl order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-tpsl-order-detail" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/orders-tpsl-detail
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>tpslId</c>"] The order id. Either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] The client order id. Either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTpSlOrder>> GetTpSlOrderAsync(string symbol, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get closed order history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-order-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/orders-history
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderType">["<c>orderType</c>"] Filter by order type</param>
        /// <param name="orderStatus">["<c>state</c>"] Filter by order status</param>
        /// <param name="afterId">["<c>before</c>"] Return results after this id</param>
        /// <param name="beforeId">["<c>after</c>"] Return results before this id</param>
        /// <param name="startTime">["<c>begin</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinOrder[]>> GetClosedOrdersAsync(string? symbol = null, OrderType? orderType = null, OrderStatus? orderStatus = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get closed tp sl order history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-tpsl-order-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/orders-tpsl-history
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETHUSDT`</param>
        /// <param name="orderId">["<c>tpslId</c>"] Filter by tpsl order id</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Filter by client order id</param>
        /// <param name="status">["<c>state</c>"] Filter by status</param>
        /// <param name="afterId">["<c>before</c>"] Return results after this id</param>
        /// <param name="beforeId">["<c>after</c>"] Return results before this id</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTpSlOrder[]>> GetClosedTpSlOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, TpSlOrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get closed trigger order history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-algo-order-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/orders-algo-history
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>algoId</c>"] Filter by algo order id</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Filter by client order id</param>
        /// <param name="status">["<c>state</c>"] Filter by status</param>
        /// <param name="afterId">["<c>before</c>"] Return results after this id</param>
        /// <param name="beforeId">["<c>after</c>"] Return results before this id</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinAlgoOrder[]>> GetClosedTriggerOrdersAsync(string? symbol = null, string? orderId = null, string? clientOrderId = null, TpSlOrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get user trade history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-trade-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/fills-history
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETHUSDT`</param>
        /// <param name="orderId">["<c>orderId</c>"] Filter by order id</param>
        /// <param name="afterId">["<c>before</c>"] Return results after this id</param>
        /// <param name="beforeId">["<c>after</c>"] Return results before this id</param>
        /// <param name="startTime">["<c>begin</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinUserTrade[]>> GetUserTradesAsync(string? symbol = null, string? orderId = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get order price limits
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-trade-order-price-range" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/order/price-range
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="side">["<c>side</c>"] Order side</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinPriceLimit>> GetPriceLimitsAsync(string symbol, OrderSide side, CancellationToken ct = default);

    }
}
