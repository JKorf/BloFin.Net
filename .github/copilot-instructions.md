# BloFin.Net Copilot Instructions

Generate code against the actual BloFin.Net client shape.

Before making API-shape decisions, also read:

- `AGENTS.md` for the full repository-specific AI coding guide
- `llms.txt` for concise BloFin.Net context
- `llms-full.txt` for detailed endpoint routing, pitfalls and examples
- `docs/ai-api-map.md` for intent-to-method mapping

## Correct Client Roots

```csharp
var restClient = new BloFinRestClient();
var socketClient = new BloFinSocketClient();

await restClient.AccountApi.GetAccountConfigAsync();
await restClient.SpotApi.ExchangeData.GetTickersAsync("ETH-USDT");
await restClient.FuturesApi.ExchangeData.GetTickersAsync("ETH-USDT");
await restClient.FuturesApi.Account.GetBalancesAsync();
await restClient.FuturesApi.Trading.GetOpenOrdersAsync("ETH-USDT");
await socketClient.SpotApi.SubscribeToTickerUpdatesAsync("ETH-USDT", update => { });
await socketClient.FuturesApi.SubscribeToTickerUpdatesAsync("ETH-USDT", update => { });
```

BloFin.Net does not expose `ExchangeApi`, `UsdFuturesApi`, `FuturesApiV2`, `SpotApiV3`, `CoinFuturesApi`, or `PerpetualFuturesApi`.

## Credentials

```csharp
options.ApiCredentials = new BloFinCredentials("API_KEY", "API_SECRET", "PASSPHRASE");
```

The passphrase is required.

## BloFin-Specific Rules

- Use `AccountApi` for general asset/account operations.
- Use `SpotApi.ExchangeData`, `SpotApi.Account`, and `SpotApi.Trading` for spot.
- Use `FuturesApi.ExchangeData`, `FuturesApi.Account`, and `FuturesApi.Trading` for futures.
- Use symbols such as `ETH-USDT`.
- Use `GetSymbolsV3Async()` only when additional native symbol metadata is needed; its BloFin endpoint is undocumented and may change.
- Use `KlineInterval`, not `BinPeriod`.
- Check `result.Success` before reading `result.Data`.
- Futures quantities are contract quantities.
- In shared futures results, read `SharedOrderQuantity.QuantityInContracts`; shared order books use `SharedQuantityType.Contracts`.
- Configure per-API behavior with `FuturesOptions` and `SpotOptions`, not `ExchangeOptions`.
- Spot user-data trackers and local order books are supported; generic kline/trade trackers remain futures-only.

## Frequent Endpoint Mapping

| Intent | BloFin.Net member |
| --- | --- |
| Account config | `restClient.AccountApi.GetAccountConfigAsync()` |
| Transfer | `restClient.AccountApi.TransferAsync(...)` |
| Spot symbols | `restClient.SpotApi.ExchangeData.GetSymbolsAsync()` |
| Spot ticker | `restClient.SpotApi.ExchangeData.GetTickersAsync(symbol)` |
| Spot order | `restClient.SpotApi.Trading.PlaceOrderAsync(...)` |
| Futures symbols | `restClient.FuturesApi.ExchangeData.GetSymbolsAsync()` |
| Tickers | `restClient.FuturesApi.ExchangeData.GetTickersAsync(symbol)` |
| Klines | `restClient.FuturesApi.ExchangeData.GetKlinesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Order book | `restClient.FuturesApi.ExchangeData.GetOrderBookAsync(symbol, depth)` |
| Futures balances | `restClient.FuturesApi.Account.GetBalancesAsync()` |
| Leverage | `restClient.FuturesApi.Account.GetLeverageAsync(...)` |
| Place order | `restClient.FuturesApi.Trading.PlaceOrderAsync(...)` |
| Open orders | `restClient.FuturesApi.Trading.GetOpenOrdersAsync(...)` |
| Positions | `restClient.FuturesApi.Trading.GetPositionsAsync(...)` |
| Ticker stream | `socketClient.FuturesApi.SubscribeToTickerUpdatesAsync(...)` |
| Order stream | `socketClient.FuturesApi.SubscribeToOrderUpdatesAsync(...)` |
| Spot ticker stream | `socketClient.SpotApi.SubscribeToTickerUpdatesAsync(...)` |

The futures and spot APIs expose shared clients. The futures shared client publishes normalized trading constraints and asset classifications to the CryptoExchange.Net shared symbol catalog.

## Avoid

- `ETHUSDT`, `ETH_USDT`, `ETH/USDT`, `tETHUSD`
- `new BloFinCredentials(key, secret)`
- `ExchangeApi`, `SpotApiV3`, `UsdFuturesApi`, `FuturesApiV2`
- `BinPeriod.OneMinute`
