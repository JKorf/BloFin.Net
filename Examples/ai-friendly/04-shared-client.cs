// 04-shared-client.cs
//
// Demonstrates: accessing CryptoExchange.Net shared clients from BloFin.Net.
//
// Setup: dotnet add package BloFin.Net

using BloFin.Net;
using BloFin.Net.Clients;

var client = new BloFinRestClient(options =>
{
    options.ApiCredentials = new BloFinCredentials("API_KEY", "API_SECRET", "PASSPHRASE");
});

var accountShared = client.AccountApi.SharedClient;
var futuresShared = client.FuturesApi.SharedClient;

Console.WriteLine($"Account shared exchange: {accountShared.Exchange}");
Console.WriteLine($"Futures shared exchange: {futuresShared.Exchange}");
Console.WriteLine($"Futures trading modes: {string.Join(", ", futuresShared.SupportedTradingModes)}");

// Native BloFin endpoints remain available beside the shared abstractions.
var nativeSymbols = await client.FuturesApi.ExchangeData.GetSymbolsAsync();
if (nativeSymbols.Success)
{
    foreach (var symbol in nativeSymbols.Data.Take(5))
        Console.WriteLine(symbol.Symbol);
}
