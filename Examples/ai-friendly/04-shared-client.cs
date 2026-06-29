// 04-shared-client.cs
//
// Demonstrates: accessing CryptoExchange.Net shared clients from BloFin.Net.
//
// Setup: dotnet add package BloFin.Net

using BloFin.Net;
using BloFin.Net.Clients;
using CryptoExchange.Net.SharedApis;

var client = new BloFinRestClient(options =>
{
    options.ApiCredentials = new BloFinCredentials("API_KEY", "API_SECRET", "PASSPHRASE");
});

var accountShared = client.AccountApi.SharedClient;
var futuresShared = client.FuturesApi.SharedClient;

var accountInfo = accountShared.Discover();
var futuresInfo = futuresShared.Discover();
var supportedFuturesFeatures = futuresInfo.Features
    .Where(x => x.Supported)
    .Select(x => x.EndpointName);

Console.WriteLine($"Account shared exchange: {accountInfo.Exchange}");
Console.WriteLine($"Futures shared exchange: {futuresInfo.Exchange}");
Console.WriteLine($"Futures trading modes: {string.Join(", ", futuresInfo.SupportedTradingModes)}");
Console.WriteLine($"Futures shared features: {string.Join(", ", supportedFuturesFeatures)}");

// Native BloFin endpoints remain available beside the shared abstractions.
var nativeSymbols = await client.FuturesApi.ExchangeData.GetSymbolsAsync();
if (nativeSymbols.Success)
{
    foreach (var symbol in nativeSymbols.Data.Take(5))
        Console.WriteLine(symbol.Symbol);
}
