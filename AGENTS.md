---
description: "AI assistant guide for BloFin.Net"
applyTo: "**/*"
---

# BloFin.Net AI Coding Guide

BloFin.Net is a CryptoExchange.Net-based client for the BloFin REST and websocket APIs. Use this file as the source of truth when generating code for this repository.

## Package And Client Shape

- NuGet package id: `BloFin.Net`
- Root namespace: `BloFin.Net`
- REST client: `BloFinRestClient`
- Socket client: `BloFinSocketClient`
- REST roots:
  - `restClient.AccountApi`
  - `restClient.FuturesApi`
- Futures REST sub-clients:
  - `restClient.FuturesApi.ExchangeData`
  - `restClient.FuturesApi.Account`
  - `restClient.FuturesApi.Trading`
- Socket root:
  - `socketClient.FuturesApi`
- Shared clients:
  - `restClient.AccountApi.SharedClient`
  - `restClient.FuturesApi.SharedClient`
  - `socketClient.FuturesApi.SharedClient`

Do not invent roots such as `SpotApi`, `SpotApiV3`, `UsdFuturesApi`, `FuturesApiV2`, `ExchangeApi`, `CoinFuturesApi`, or `PerpetualFuturesApi`. BloFin.Net uses `AccountApi` for asset/account endpoints and `FuturesApi` for market/trading/futures account endpoints.

## Credentials And Options

BloFin credentials require API key, API secret and passphrase:

```csharp
var restClient = new BloFinRestClient(options =>
{
    options.ApiCredentials = new BloFinCredentials("API_KEY", "API_SECRET", "PASSPHRASE");
});
```

The README notes that BloFin API keys should be created as `Connect to Third-Party Application` with `JKorf` selected as the application name.

Useful options:

- `BloFinRestOptions.Environment` defaults to `BloFinEnvironment.Live`
- `BloFinRestOptions.BrokerId`
- `BloFinRestOptions.ExchangeOptions`
- `BloFinSocketOptions.Environment` defaults to `BloFinEnvironment.Live`
- `BloFinSocketOptions.SocketSubscriptionsCombineTarget` defaults to `10`
- `BloFinSocketOptions.ExchangeOptions`
- Use `BloFinRestClient.SetDefaultOptions(...)` and `BloFinSocketClient.SetDefaultOptions(...)` for application-wide defaults
- Use `services.AddBloFin(...)` for dependency injection

## Symbol Rules

BloFin futures symbols use hyphen-separated instrument ids:

- `BTC-USDT`
- `ETH-USDT`

Do not rewrite symbols into:

- `BTCUSDT`
- `BTC_USDT`
- `BTC/USDT`
- `tBTCUSD`

Use `restClient.FuturesApi.ExchangeData.GetSymbolsAsync()` to discover supported instruments.

## REST Endpoint Routing

General account endpoints are under `restClient.AccountApi`:

- `GetAccountBalancesAsync(AccountType accountType, string? asset = null)`
- `TransferAsync(asset, fromAccount, toAccount, quantity, clientId)`
- `GetAccountConfigAsync()`
- `GetApiKeyInfoAsync()`
- `GetTransferHistoryAsync(...)`
- `GetWithdrawalHistoryAsync(...)`
- `GetDepositHistoryAsync(...)`

Futures exchange data is under `restClient.FuturesApi.ExchangeData`:

- `GetSymbolsAsync(symbol)`
- `GetSymbolsV3Async()`
- `GetTickersAsync(symbol)`
- `GetOrderBookAsync(symbol, depth)`
- `GetRecentTradesAsync(symbol, limit)`
- `GetIndexMarkPriceAsync(symbol)`
- `GetFundingRateAsync(symbol)`
- `GetFundingRateHistoryAsync(symbol, ...)`
- `GetKlinesAsync(symbol, KlineInterval.OneMinute, ...)`
- `GetIndexPriceKlinesAsync(symbol, KlineInterval.OneMinute, ...)`
- `GetMarkPriceKlinesAsync(symbol, KlineInterval.OneMinute, ...)`
- `GetPositionTiersAsync(symbol, marginMode)`

Futures account endpoints are under `restClient.FuturesApi.Account`:

- `GetBalancesAsync(productType)`
- `GetMarginModeAsync()`
- `SetMarginModeAsync(marginMode)`
- `GetPositionModeAsync()`
- `SetPositionModeAsync(positionMode)`
- `GetLeverageAsync(symbol, marginMode)`
- `SetLeverageAsync(symbol, leverage, marginMode, positionSide)`

Futures trading endpoints are under `restClient.FuturesApi.Trading`:

- `GetPositionsAsync(symbol)`
- `PlaceOrderAsync(symbol, side, orderType, quantity, marginMode, price, positionSide, reduceOnly, clientOrderId, ...)`
- `PlaceMultipleOrdersAsync(orders)`
- `PlaceTpSlOrderAsync(...)`
- `PlaceTriggerOrderAsync(...)`
- `CancelOrderAsync(orderId, symbol, clientOrderId)`
- `CancelOrdersAsync(orders)`
- `CancelTpSlOrdersAsync(orders)`
- `CancelTriggerOrderAsync(orderId, symbol, clientOrderId)`
- `GetOpenOrdersAsync(...)`
- `GetOpenTpSlOrdersAsync(...)`
- `GetOpenTriggerOrdersAsync(...)`
- `ClosePositionAsync(symbol, marginMode, positionSide, clientOrderId)`
- `GetOrderAsync(symbol, orderId, clientOrderId)`
- `GetTpSlOrderAsync(symbol, orderId, clientOrderId)`
- `GetClosedOrdersAsync(...)`
- `GetClosedTpSlOrdersAsync(...)`
- `GetClosedTriggerOrdersAsync(...)`
- `GetUserTradesAsync(...)`
- `GetPriceLimitsAsync(symbol, side)`
- `GetPositionHistoryAsync(...)`

## Websocket Routing

Use `socketClient.FuturesApi`.

Public subscriptions:

- `SubscribeToTradeUpdatesAsync(symbol, ...)`
- `SubscribeToKlineUpdatesAsync(symbol, KlineInterval.OneMinute, ...)`
- `SubscribeToIndexPriceKlineUpdatesAsync(symbol, KlineInterval.OneMinute, ...)`
- `SubscribeToMarkPriceKlineUpdatesAsync(symbol, KlineInterval.OneMinute, ...)`
- `SubscribeToOrderBookUpdatesAsync(symbol, depth, ...)`
- `SubscribeToTickerUpdatesAsync(symbol, ...)`
- `SubscribeToFundingRateUpdatesAsync(symbol, ...)`

Private subscriptions:

- `SubscribeToPositionUpdatesAsync(...)`
- `SubscribeToOrderUpdatesAsync(...)`
- `SubscribeToTriggerOrderUpdatesAsync(...)`
- `SubscribeToBalanceUpdatesAsync(...)`
- `SubscribeToInverseBalanceUpdatesAsync(...)`

Always check `subscription.Success` before using `subscription.Data`. Unsubscribe with `await socketClient.UnsubscribeAsync(subscription.Data)`.

## Enums To Use

- Account types: `AccountType`
- Products: `ProductType`
- Klines: `KlineInterval.OneMinute`, `ThreeMinutes`, `FiveMinutes`, `FifteenMinutes`, `OneHour`, `OneDay`
- Orders: `OrderSide.Buy`, `OrderSide.Sell`
- Order types: `OrderType.Market`, `Limit`, `PostOnly`, `FillOrKill`, `ImmediateOrCancel`
- Margin: `MarginMode.Cross`, `MarginMode.Isolated`
- Position mode: `PositionMode.OneWayMode`, `PositionMode.HedgeMode`
- Position side: `PositionSide.Long`, `Short`, `Net`
- Trigger price: `TriggerPriceType`

## Result Handling

REST calls return `HttpResult<T>` and socket subscriptions return `WebSocketResult<UpdateSubscription>`.

```csharp
var result = await client.FuturesApi.ExchangeData.GetTickersAsync("ETH-USDT");
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}

Console.WriteLine(result.Data.FirstOrDefault()?.LastPrice);
```

Never assume `Data` is populated when `Success` is false.

## Shared Futures Symbol Metadata

The futures shared client publishes its symbols to the CryptoExchange.Net shared symbol catalog. Shared futures symbols include trading constraints and asset classifications such as crypto, equity and commodity where BloFin's V3 metadata identifies them. Prefer the shared catalog when multi-exchange code needs normalized symbol or asset-type metadata.

## Local Order Book And Trackers

BloFin.Net includes:

- `BloFinOrderBookFactory`
- `BloFinSymbolOrderBook`
- `BloFinTrackerFactory`
- `BloFinUserFuturesDataTracker`

Use these when code needs maintained local order book state or user data tracking instead of manually combining snapshots and websocket deltas.

## Common Pitfalls

- Do not use `ExchangeApi`; BloFin REST uses `AccountApi` and `FuturesApi`.
- Do not use `SpotApi`, `UsdFuturesApi`, `FuturesApiV2`, or versioned API roots.
- Do not omit the passphrase when creating `BloFinCredentials`.
- Do not convert `ETH-USDT` to `ETHUSDT`, `ETH_USDT`, or `ETH/USDT`.
- Use `KlineInterval`, not `BinPeriod`.
- Futures order quantities are contract sizes; do not assume spot/base-asset semantics.

## Source Files To Inspect Before Changing API Usage

- `BloFin.Net/Interfaces/Clients/Apis/IBloFinRestClientAccountApi.cs`
- `BloFin.Net/Interfaces/Clients/ExchangeApi/IBloFinRestClientFuturesApiExchangeData.cs`
- `BloFin.Net/Interfaces/Clients/ExchangeApi/IBloFinRestClientFuturesApiAccount.cs`
- `BloFin.Net/Interfaces/Clients/ExchangeApi/IBloFinRestClientFuturesApiTrading.cs`
- `BloFin.Net/Interfaces/Clients/ExchangeApi/IBloFinSocketClientExchangeApi.cs`
- `BloFin.Net/Objects/Options/BloFinRestOptions.cs`
- `BloFin.Net/Objects/Options/BloFinSocketOptions.cs`
- `BloFin.Net/BloFinCredentials.cs`
- `Examples/ai-friendly`
