// 04-shared-client.cs
//
// Demonstrates: accessing CryptoExchange.Net Shared API V2 capabilities from BloFin.Net.
//
// Setup: dotnet add package BloFin.Net

using BloFin.Net;
using BloFin.Net.Clients;
using CryptoExchange.Net.SharedApis;

using var client = new BloFinRestClient(options =>
{
    options.ApiCredentials = new BloFinCredentials("API_KEY", "API_SECRET", "PASSPHRASE");
});

var accountShared = client.AccountApi.SharedApi;
var futuresShared = client.FuturesApi.SharedApi;

var accountInfo = accountShared.Discover();
var futuresInfo = futuresShared.Discover();
var supportedFuturesFeatures = futuresInfo.Features
    .Where(x => x.Supported)
    .Select(x => x.EndpointName);

Console.WriteLine($"Account shared exchange: {accountInfo.Exchange}");
Console.WriteLine($"Futures shared exchange: {futuresInfo.Exchange}");
Console.WriteLine($"Futures trading modes: {string.Join(", ", futuresInfo.SupportedTradingModes)}");
Console.WriteLine($"Futures shared features: {string.Join(", ", supportedFuturesFeatures)}");

// Query through a narrow V2 capability and populate its normalized symbol catalog.
IGetFuturesSymbolsRest symbolsClient = futuresShared;
var symbols = await symbolsClient.GetFuturesSymbolsAsync(new GetSymbolsRequest());
if (!symbols.Success)
{
    Console.WriteLine(symbols.Error);
    return;
}
foreach (var symbol in symbols.Data.Take(5))
    Console.WriteLine($"{symbol.Name}: contract size={symbol.ContractSize}");
var catalog = symbolsClient.FuturesSymbolCatalog;

// Native BloFin endpoints remain available beside the shared abstractions.
var nativeSymbols = await client.FuturesApi.ExchangeData.GetSymbolsAsync();
if (nativeSymbols.Success)
{
    foreach (var symbol in nativeSymbols.Data.Take(5))
        Console.WriteLine(symbol.Symbol);
}
