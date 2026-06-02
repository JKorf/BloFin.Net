using BloFin.Net;
using BloFin.Net.Clients;
using BloFin.Net.Enums;

const string symbol = "ETH-USDT";

// Replace with valid credentials or order placement will always fail
var apiKey = "KEY";
var apiSecret = "SECRET";
var apiPassphrase = "PASSPHRASE";

Console.WriteLine("BloFin.Net order placement example");
Console.WriteLine();
Console.WriteLine("This example can place real orders when valid credentials are configured.");
Console.WriteLine();

var client = new BloFinRestClient(options =>
{
    options.ApiCredentials = new BloFinCredentials(apiKey, apiSecret, apiPassphrase);
});

await PlaceFuturesLimitOrderAsync(client);

static async Task PlaceFuturesLimitOrderAsync(BloFinRestClient client)
{
    Console.WriteLine($"Placing futures limit buy order for {symbol}...");

    var ticker = await client.FuturesApi.ExchangeData.GetTickersAsync(symbol);
    if (!ticker.Success)
    {
        Console.WriteLine($"Failed to get ticker: {ticker.Error}");
        return;
    }

    var tickerData = ticker.Data.SingleOrDefault();
    if (tickerData == null)
    {
        Console.WriteLine($"Ticker not found for {symbol}");
        return;
    }

    var safePrice = Math.Round(tickerData.LastPrice * 0.95m, 2);
    var order = await client.FuturesApi.Trading.PlaceOrderAsync(
        symbol: symbol,
        side: OrderSide.Buy,
        orderType: OrderType.Limit,
        quantity: 0.01m,
        marginMode: MarginMode.Cross,
        price: safePrice);

    if (!order.Success)
    {
        Console.WriteLine($"Failed to place order: {order.Error}");
        return;
    }

    Console.WriteLine($"Placed order {order.Data.OrderId}");

    var orderStatus = await client.FuturesApi.Trading.GetOrderAsync(symbol, order.Data.OrderId);
    if (orderStatus.Success)
        Console.WriteLine($"Order status: {orderStatus.Data.Status}, filled: {orderStatus.Data.QuantityFilled}");
    else
        Console.WriteLine($"Failed to query order: {orderStatus.Error}");

    var cancel = await client.FuturesApi.Trading.CancelOrderAsync(order.Data.OrderId, symbol);
    Console.WriteLine(cancel.Success
        ? $"Cancelled order {order.Data.OrderId}"
        : $"Failed to cancel order: {cancel.Error}");
}
