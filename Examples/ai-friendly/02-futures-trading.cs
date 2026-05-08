// 02-futures-trading.cs
//
// Demonstrates: BloFin futures limit order flow, open orders, cancellation and positions.
//
// Setup: dotnet add package BloFin.Net

using BloFin.Net;
using BloFin.Net.Clients;
using BloFin.Net.Enums;

var client = new BloFinRestClient(options =>
{
    options.ApiCredentials = new BloFinCredentials("API_KEY", "API_SECRET", "PASSPHRASE");
});

const string symbol = "ETH-USDT";

var order = await client.FuturesApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    side: OrderSide.Buy,
    orderType: OrderType.Limit,
    quantity: 1m,
    marginMode: MarginMode.Cross,
    price: 1m,
    positionSide: PositionSide.Long,
    clientOrderId: $"ai-example-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");

if (!order.Success)
{
    Console.WriteLine($"Order rejected: {order.Error}");
    return;
}

Console.WriteLine($"Placed order {order.Data.OrderId}");

var openOrders = await client.FuturesApi.Trading.GetOpenOrdersAsync(symbol);
if (openOrders.Success)
    Console.WriteLine($"Open orders on {symbol}: {openOrders.Data.Length}");

var cancel = await client.FuturesApi.Trading.CancelOrderAsync(orderId: order.Data.OrderId);
Console.WriteLine(cancel.Success
    ? $"Canceled order {cancel.Data.OrderId}"
    : $"Cancel failed: {cancel.Error}");

var positions = await client.FuturesApi.Trading.GetPositionsAsync(symbol);
if (positions.Success)
{
    foreach (var position in positions.Data)
        Console.WriteLine($"{position.Symbol}: side={position.PositionSide}, size={position.PositionSize}, leverage={position.Leverage}");
}

var leverage = await client.FuturesApi.Account.GetLeverageAsync(symbol, MarginMode.Cross);
if (leverage.Success)
    Console.WriteLine($"Leverage rows: {leverage.Data.Length}");
