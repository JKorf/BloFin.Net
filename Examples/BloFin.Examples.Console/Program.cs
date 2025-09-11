
using BloFin.Net.Clients;

// REST
var restClient = new BloFinRestClient();
var ticker = await restClient.FuturesApi.ExchangeData.GetTickersAsync("ETH-USDT");
Console.WriteLine($"Rest client ticker price for ETHUSDT: {ticker.Data.First().LastPrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
var socketClient = new BloFinSocketClient();
var subscription = await socketClient.FuturesApi.SubscribeToTickerUpdatesAsync("ETH-USDT", update =>
{
    Console.WriteLine($"Websocket client ticker price for ETH-USDT: {update.Data.LastPrice}");
});

Console.ReadLine();
