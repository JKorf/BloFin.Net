# Usage examples

Snippets are independent. Those using `client` assume a reused `BloFinRestClient`, configured with credentials for private operations. Calls that submit orders can execute real trades; review [operational considerations](safety.md) before running them.

## Imports

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using BloFin.Net;
using BloFin.Net.Clients;
using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
```

## Public futures data and result handling

```csharp
using var client = new BloFinRestClient();
var result = await client.FuturesApi.ExchangeData.GetTickersAsync("ETH-USDT");
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.FirstOrDefault()?.LastPrice);
```

## Public spot data

```csharp
var result = await client.SpotApi.ExchangeData.GetSymbolsAsync("ETH-USDT");
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
foreach (var symbol in result.Data)
    Console.WriteLine($"{symbol.Symbol}: minimum={symbol.MinQuantity}, lot={symbol.LotSize}, tick={symbol.TickSize}");
```

## Authentication and general account balances

```csharp
using var client = new BloFinRestClient(options =>
{
    options.ApiCredentials = new BloFinCredentials("API_KEY", "API_SECRET", "PASSPHRASE");
});
var balances = await client.AccountApi.GetAccountBalancesAsync(AccountType.Futures, "USDT");
if (!balances.Success)
{
    Console.WriteLine(balances.Error);
    return;
}
foreach (var balance in balances.Data)
    Console.WriteLine($"{balance.Asset}: {balance.AvailableBalance}");
```

## Spot account balances

```csharp
var balances = await client.SpotApi.Account.GetBalancesAsync(AccountType.Spot, asset: "USDT");
if (!balances.Success)
{
    Console.WriteLine(balances.Error);
    return;
}
foreach (var balance in balances.Data)
    Console.WriteLine($"{balance.Asset}: {balance.AvailableBalance}");
```

## Spot limit order and cancellation

```csharp
var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: "ETH-USDT", side: OrderSide.Buy, type: OrderType.Limit,
    quantity: 0.01m, price: 1m);
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
var cancel = await client.SpotApi.Trading.CancelOrderAsync(orderId: order.Data.OrderId);
if (!cancel.Success)
    Console.WriteLine(cancel.Error);
```

## Futures limit order and cancellation

This example uses contracts and a long position side for a matching hedge-mode account. Select the position side and margin mode that match the intended account workflow.

```csharp
var order = await client.FuturesApi.Trading.PlaceOrderAsync(
    symbol: "ETH-USDT", side: OrderSide.Buy, orderType: OrderType.Limit,
    quantity: 1m, marginMode: MarginMode.Cross, price: 1m,
    positionSide: PositionSide.Long);
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
var cancel = await client.FuturesApi.Trading.CancelOrderAsync(orderId: order.Data.OrderId);
if (!cancel.Success)
    Console.WriteLine(cancel.Error);
```

## Positions and leverage reads

```csharp
var positions = await client.FuturesApi.Trading.GetPositionsAsync("ETH-USDT");
if (!positions.Success)
{
    Console.WriteLine(positions.Error);
    return;
}
foreach (var position in positions.Data)
    Console.WriteLine($"{position.Symbol}: {position.PositionSide}, size={position.PositionSize}");
var leverage = await client.FuturesApi.Account.GetLeverageAsync("ETH-USDT", MarginMode.Cross);
if (!leverage.Success)
    Console.WriteLine(leverage.Error);
```

## WebSocket lifecycle

Both spot and futures expose ticker subscriptions. Configure credentials on the socket client for private streams.

```csharp
using var socketClient = new BloFinSocketClient();
var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "ETH-USDT", update => Console.WriteLine(update.Data.LastPrice));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socketClient.UnsubscribeAsync(subscription.Data);
}
```

## Shared API V2

```csharp
IGetTickerRest ticker = client.FuturesApi.SharedApi;
var symbol = new SharedSymbol(TradingMode.PerpetualLinear, "ETH", "USDT");
var result = await ticker.GetTickerAsync(new GetTickerRequest(symbol));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

Use `client.SpotApi.SharedApi` and `TradingMode.Spot` for a shared spot ticker. Read [API surfaces](api-surfaces.md#shared-api-v2) for runtime selection and catalog prerequisites.

## Dependency injection

```csharp
var services = new ServiceCollection();
services.AddBloFin(options =>
{
    options.ApiCredentials = new BloFinCredentials("API_KEY", "API_SECRET", "PASSPHRASE");
});
using var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IBloFinRestClient>();
var shared = provider.GetRequiredService<IBloFinSharedApiClient>();
```

## Complete examples (checkout-only)

The documentation test compiles these programs without executing exchange operations. Running them can contact BloFin and execute their illustrated account operations.

- [Futures market and account](../../../../Examples/ai-friendly/01-futures-market-and-account.cs)
- [Futures trading](../../../../Examples/ai-friendly/02-futures-trading.cs)
- [WebSockets](../../../../Examples/ai-friendly/03-websocket.cs)
- [Shared API](../../../../Examples/ai-friendly/04-shared-client.cs)
- [Error handling](../../../../Examples/ai-friendly/05-error-handling.cs)
