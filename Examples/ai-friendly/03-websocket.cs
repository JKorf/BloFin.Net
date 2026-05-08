// 03-websocket.cs
//
// Demonstrates: BloFin public and private futures websocket subscriptions.
//
// Setup: dotnet add package BloFin.Net

using BloFin.Net;
using BloFin.Net.Clients;
using BloFin.Net.Enums;

var socketClient = new BloFinSocketClient(options =>
{
    options.ApiCredentials = new BloFinCredentials("API_KEY", "API_SECRET", "PASSPHRASE");
});

const string symbol = "ETH-USDT";

var tickerSubscription = await socketClient.FuturesApi.SubscribeToTickerUpdatesAsync(
    symbol,
    update =>
    {
        Console.WriteLine($"{update.Data.Symbol} ticker: {update.Data.LastPrice}");
    });

if (!tickerSubscription.Success)
{
    Console.WriteLine($"Ticker subscription failed: {tickerSubscription.Error}");
    return;
}

var klineSubscription = await socketClient.FuturesApi.SubscribeToKlineUpdatesAsync(
    symbol,
    KlineInterval.OneMinute,
    update =>
    {
        Console.WriteLine($"{symbol} 1m close: {update.Data.ClosePrice}");
    });

if (!klineSubscription.Success)
{
    Console.WriteLine($"Kline subscription failed: {klineSubscription.Error}");
    await socketClient.UnsubscribeAsync(tickerSubscription.Data);
    return;
}

var orderSubscription = await socketClient.FuturesApi.SubscribeToOrderUpdatesAsync(update =>
{
    foreach (var order in update.Data)
        Console.WriteLine($"Order update: {order.OrderId} {order.Status}");
});

if (!orderSubscription.Success)
{
    Console.WriteLine($"Private order subscription failed: {orderSubscription.Error}");
    await socketClient.UnsubscribeAsync(tickerSubscription.Data);
    await socketClient.UnsubscribeAsync(klineSubscription.Data);
    return;
}

Console.WriteLine("Listening. Press Enter to unsubscribe.");
Console.ReadLine();

await socketClient.UnsubscribeAsync(tickerSubscription.Data);
await socketClient.UnsubscribeAsync(klineSubscription.Data);
await socketClient.UnsubscribeAsync(orderSubscription.Data);
