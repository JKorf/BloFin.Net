# BloFin.Net AI API Map

This map helps AI assistants route common user intents to the actual BloFin.Net methods.

## Client Roots

| Need | Use |
| --- | --- |
| REST client | `new BloFinRestClient(...)` |
| Socket client | `new BloFinSocketClient(...)` |
| General account and asset endpoints | `restClient.AccountApi` |
| Futures market data | `restClient.FuturesApi.ExchangeData` |
| Futures account settings | `restClient.FuturesApi.Account` |
| Futures trading | `restClient.FuturesApi.Trading` |
| Futures websocket subscriptions | `socketClient.FuturesApi` |
| Shared REST abstraction | `restClient.AccountApi.SharedClient` / `restClient.FuturesApi.SharedClient` |
| Shared socket abstraction | `socketClient.FuturesApi.SharedClient` |
| Shared capability discovery | `restClient.AccountApi.SharedClient.Discover()` / `restClient.FuturesApi.SharedClient.Discover()` / `socketClient.FuturesApi.SharedClient.Discover()` |

## AccountApi

| Intent | Method |
| --- | --- |
| Account balances | `AccountApi.GetAccountBalancesAsync(accountType, asset)` |
| Transfer funds | `AccountApi.TransferAsync(asset, fromAccount, toAccount, quantity, clientId)` |
| Account config | `AccountApi.GetAccountConfigAsync()` |
| API key info | `AccountApi.GetApiKeyInfoAsync()` |
| Transfer history | `AccountApi.GetTransferHistoryAsync(...)` |
| Withdrawal history | `AccountApi.GetWithdrawalHistoryAsync(...)` |
| Deposit history | `AccountApi.GetDepositHistoryAsync(...)` |

## Futures Market Data

| Intent | Method |
| --- | --- |
| Symbols | `FuturesApi.ExchangeData.GetSymbolsAsync()` |
| Tickers | `FuturesApi.ExchangeData.GetTickersAsync(symbol)` |
| Order book | `FuturesApi.ExchangeData.GetOrderBookAsync(symbol, depth)` |
| Recent trades | `FuturesApi.ExchangeData.GetRecentTradesAsync(symbol, limit)` |
| Index/mark price | `FuturesApi.ExchangeData.GetIndexMarkPriceAsync(symbol)` |
| Funding rate | `FuturesApi.ExchangeData.GetFundingRateAsync(symbol)` |
| Funding history | `FuturesApi.ExchangeData.GetFundingRateHistoryAsync(symbol, ...)` |
| Klines | `FuturesApi.ExchangeData.GetKlinesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Index klines | `FuturesApi.ExchangeData.GetIndexPriceKlinesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Mark klines | `FuturesApi.ExchangeData.GetMarkPriceKlinesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Position tiers | `FuturesApi.ExchangeData.GetPositionTiersAsync(symbol, marginMode)` |

## Futures Account

| Intent | Method |
| --- | --- |
| Futures balances | `FuturesApi.Account.GetBalancesAsync(productType)` |
| Margin mode | `FuturesApi.Account.GetMarginModeAsync()` |
| Set margin mode | `FuturesApi.Account.SetMarginModeAsync(marginMode)` |
| Position mode | `FuturesApi.Account.GetPositionModeAsync()` |
| Set position mode | `FuturesApi.Account.SetPositionModeAsync(positionMode)` |
| Leverage settings | `FuturesApi.Account.GetLeverageAsync(symbol, marginMode)` |
| Set leverage | `FuturesApi.Account.SetLeverageAsync(symbol, leverage, marginMode, positionSide)` |

## Futures Trading

| Intent | Method |
| --- | --- |
| Positions | `FuturesApi.Trading.GetPositionsAsync(symbol)` |
| Place order | `FuturesApi.Trading.PlaceOrderAsync(...)` |
| Place batch orders | `FuturesApi.Trading.PlaceMultipleOrdersAsync(orders)` |
| Place TP/SL | `FuturesApi.Trading.PlaceTpSlOrderAsync(...)` |
| Place trigger order | `FuturesApi.Trading.PlaceTriggerOrderAsync(...)` |
| Cancel order | `FuturesApi.Trading.CancelOrderAsync(orderId: ...)` or `CancelOrderAsync(symbol: ..., clientOrderId: ...)` |
| Open orders | `FuturesApi.Trading.GetOpenOrdersAsync(...)` |
| Open TP/SL orders | `FuturesApi.Trading.GetOpenTpSlOrdersAsync(...)` |
| Open trigger orders | `FuturesApi.Trading.GetOpenTriggerOrdersAsync(...)` |
| Close position | `FuturesApi.Trading.ClosePositionAsync(...)` |
| Order details | `FuturesApi.Trading.GetOrderAsync(...)` |
| Closed orders | `FuturesApi.Trading.GetClosedOrdersAsync(...)` |
| User trades | `FuturesApi.Trading.GetUserTradesAsync(...)` |
| Price limits | `FuturesApi.Trading.GetPriceLimitsAsync(symbol, side)` |
| Position history | `FuturesApi.Trading.GetPositionHistoryAsync(...)` |

## Sockets

| Intent | Method |
| --- | --- |
| Trades | `SubscribeToTradeUpdatesAsync(symbol, ...)` |
| Klines | `SubscribeToKlineUpdatesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Order book | `SubscribeToOrderBookUpdatesAsync(symbol, depth, ...)` |
| Tickers | `SubscribeToTickerUpdatesAsync(symbol, ...)` |
| Funding | `SubscribeToFundingRateUpdatesAsync(symbol, ...)` |
| Private positions | `SubscribeToPositionUpdatesAsync(...)` |
| Private orders | `SubscribeToOrderUpdatesAsync(...)` |
| Private trigger orders | `SubscribeToTriggerOrderUpdatesAsync(...)` |
| Private balances | `SubscribeToBalanceUpdatesAsync(...)` |

## Shared API Result Handling

| Situation | Pattern |
| --- | --- |
| REST success check | `if (!result.Success) { Console.WriteLine(result.Error); return; }` |
| Socket subscription success check | `WebSocketResult<UpdateSubscription> sub = await ...; if (!sub.Success) { Console.WriteLine(sub.Error); return; }` |
| Read REST data | Read `result.Data` only after `result.Success` |
| Shared helper data | Read `ExchangeCallResult<T>.Data` only after `result.Success` |
| Capability discovery | Call `.Discover()` on a shared client and inspect `Features.Where(x => x.Supported)` |

Shared REST calls return `HttpResult<T>` / `HttpResult`. Shared socket subscriptions return `WebSocketResult<UpdateSubscription>`. Shared non-I/O symbol/cache helpers such as symbol support checks return `ExchangeCallResult<T>`.

## Avoid / Replace

| Avoid | Use |
| --- | --- |
| `ExchangeApi` | `AccountApi` or `FuturesApi` |
| `SpotApi` | `FuturesApi` where applicable |
| `UsdFuturesApi` | `FuturesApi` |
| `FuturesApiV2` | `FuturesApi` |
| `BinPeriod.OneMinute` | `KlineInterval.OneMinute` |
| `ETHUSDT`, `ETH_USDT`, `ETH/USDT`, `tETHUSD` | BloFin-native symbols such as `ETH-USDT` |
| `BloFinCredentials(key, secret)` | `BloFinCredentials(key, secret, passphrase)` |
