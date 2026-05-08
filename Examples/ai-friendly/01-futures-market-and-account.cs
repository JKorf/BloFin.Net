// 01-futures-market-and-account.cs
//
// Demonstrates: BloFin futures market data, general account balances and futures balances.
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

var symbols = await client.FuturesApi.ExchangeData.GetSymbolsAsync(symbol);
if (!symbols.Success)
{
    Console.WriteLine($"Symbols failed: {symbols.Error}");
    return;
}

foreach (var item in symbols.Data)
    Console.WriteLine($"{item.Symbol}");

var ticker = await client.FuturesApi.ExchangeData.GetTickersAsync(symbol);
if (!ticker.Success)
{
    Console.WriteLine($"Ticker failed: {ticker.Error}");
    return;
}

var firstTicker = ticker.Data.FirstOrDefault();
Console.WriteLine($"{firstTicker?.Symbol} last={firstTicker?.LastPrice}, bid={firstTicker?.BestBidPrice}, ask={firstTicker?.BestAskPrice}");

var book = await client.FuturesApi.ExchangeData.GetOrderBookAsync(symbol, depth: 20);
if (!book.Success)
{
    Console.WriteLine($"Order book failed: {book.Error}");
    return;
}

Console.WriteLine($"{symbol} book levels: bids={book.Data.Bids.Length}, asks={book.Data.Asks.Length}");

var candles = await client.FuturesApi.ExchangeData.GetKlinesAsync(symbol, KlineInterval.OneMinute, limit: 5);
if (candles.Success)
{
    foreach (var candle in candles.Data)
        Console.WriteLine($"{candle.OpenTime:u} open={candle.OpenPrice} close={candle.ClosePrice}");
}

var accountBalances = await client.AccountApi.GetAccountBalancesAsync(AccountType.Futures, "USDT");
if (accountBalances.Success)
{
    foreach (var balance in accountBalances.Data)
        Console.WriteLine($"{balance.Asset}: total={balance.TotalBalance}, available={balance.AvailableBalance}");
}

var futuresBalances = await client.FuturesApi.Account.GetBalancesAsync(ProductType.UsdtFutures);
if (futuresBalances.Success)
{
    Console.WriteLine($"Futures total equity: {futuresBalances.Data.TotalEquity}");
}
