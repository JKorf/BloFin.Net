// 05-error-handling.cs
//
// Demonstrates: reusable BloFin.Net REST and socket result handling.
//
// Setup: dotnet add package BloFin.Net

using BloFin.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

var restClient = new BloFinRestClient();
var socketClient = new BloFinSocketClient();

var book = await restClient.FuturesApi.ExchangeData.GetOrderBookAsync("ETH-USDT", 20);
if (!EnsureSuccess(book, "load order book"))
    return;

Console.WriteLine($"Best bid: {book.Data.Bids.FirstOrDefault()?.Price}");

var subscription = await socketClient.FuturesApi.SubscribeToOrderBookUpdatesAsync(
    "ETH-USDT",
    5,
    update =>
    {
        Console.WriteLine($"Book update: bids={update.Data.Bids.Length}, asks={update.Data.Asks.Length}");
    });

if (!EnsureSuccessSocket(subscription, "subscribe to order book"))
    return;

await socketClient.UnsubscribeAsync(subscription.Data);

static bool EnsureSuccess<T>(WebCallResult<T> result, string action)
{
    if (result.Success)
        return true;

    Console.WriteLine($"Could not {action}: {result.Error}");
    return false;
}

static bool EnsureSuccessSocket<T>(CallResult<T> result, string action)
{
    if (result.Success)
        return true;

    Console.WriteLine($"Could not {action}: {result.Error}");
    return false;
}
