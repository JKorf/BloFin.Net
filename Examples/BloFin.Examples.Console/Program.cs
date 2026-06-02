using BloFin.Net.Clients;

// REST
var restClient = new BloFinRestClient();
var ticker = await restClient.FuturesApi.ExchangeData.GetTickersAsync("ETH-USDT");
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

var tickerData = ticker.Data.FirstOrDefault();
if (tickerData == null)
{
    Console.WriteLine("Ticker not found for ETH-USDT");
    return;
}

Console.WriteLine($"Rest client ticker price for ETH-USDT: {tickerData.LastPrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
var socketClient = new BloFinSocketClient();
var subscription = await socketClient.FuturesApi.SubscribeToTickerUpdatesAsync("ETH-USDT", update =>
{
    Console.WriteLine($"Websocket client ticker price for ETH-USDT: {update.Data.LastPrice}");
});

if (!subscription.Success)
{
    Console.WriteLine($"Failed to subscribe to ticker updates: {subscription.Error}");
    return;
}

Console.ReadLine();
