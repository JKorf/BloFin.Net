# API surfaces

## Client roots and symbols

`client` below is a `BloFinRestClient`, and `socketClient` is a `BloFinSocketClient`. REST has general `AccountApi`, plus `SpotApi` and `FuturesApi` with `ExchangeData`, `Account`, and `Trading` groups. Sockets expose `SpotApi` and `FuturesApi` directly.

Spot and futures native symbols use instrument IDs such as `ETH-USDT`. Query symbols for the selected market; futures quantities are contract counts. Candles use `KlineInterval`, product selection uses `ProductType`, account selection uses `AccountType`, and futures margin and position settings use `MarginMode`, `PositionMode`, and `PositionSide`.

## AccountApi

| Intent | Method |
| --- | --- |
| Account balances | `client.AccountApi.GetAccountBalancesAsync(accountType, asset)` |
| Transfer funds | `client.AccountApi.TransferAsync(asset, fromAccount, toAccount, quantity, clientId)` |
| Account config | `client.AccountApi.GetAccountConfigAsync()` |
| API key info | `client.AccountApi.GetApiKeyInfoAsync()` |
| Transfer history | `client.AccountApi.GetTransferHistoryAsync(...)` |
| Withdrawal history | `client.AccountApi.GetWithdrawalHistoryAsync(...)` |
| Deposit history | `client.AccountApi.GetDepositHistoryAsync(...)` |

## Spot

| Intent | Method |
| --- | --- |
| Symbols | `client.SpotApi.ExchangeData.GetSymbolsAsync()` |
| Tickers | `client.SpotApi.ExchangeData.GetTickersAsync(symbol)` |
| Order book | `client.SpotApi.ExchangeData.GetOrderBookAsync(symbol, depth)` |
| Recent trades | `client.SpotApi.ExchangeData.GetRecentTradesAsync(symbol, limit)` |
| Klines | `client.SpotApi.ExchangeData.GetKlinesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Balances | `client.SpotApi.Account.GetBalancesAsync(accountType, asset)` |
| Place order | `client.SpotApi.Trading.PlaceOrderAsync(...)` |
| Cancel order | `client.SpotApi.Trading.CancelOrderAsync(...)` |
| Open orders | `client.SpotApi.Trading.GetOpenOrdersAsync(...)` |
| Order details/history | `client.SpotApi.Trading.GetOrderAsync(...)` / `client.SpotApi.Trading.GetOrderHistoryAsync(...)` |
| User trades | `client.SpotApi.Trading.GetUserTradesAsync(...)` |
| Transfer | `client.SpotApi.Account.TransferAsync(...)` |
| Transfer history | `client.SpotApi.Account.GetTransferHistoryAsync(...)` |
| Place batch orders | `client.SpotApi.Trading.PlaceMultipleOrdersAsync(...)` |

### Additional spot algo workflows

| Intent | Method |
| --- | --- |
| Place algo order | `client.SpotApi.Trading.PlaceAlgoOrderAsync(...)` |
| Cancel algo order | `client.SpotApi.Trading.CancelAlgoOrderAsync(...)` |
| Open algo orders | `client.SpotApi.Trading.GetOpenAlgoOrdersAsync(...)` |
| Algo order history | `client.SpotApi.Trading.GetAlgoOrderHistoryAsync(...)` |

## Futures Market Data

| Intent | Method |
| --- | --- |
| Symbols | `client.FuturesApi.ExchangeData.GetSymbolsAsync()` |
| Tickers | `client.FuturesApi.ExchangeData.GetTickersAsync(symbol)` |
| Order book | `client.FuturesApi.ExchangeData.GetOrderBookAsync(symbol, depth)` |
| Recent trades | `client.FuturesApi.ExchangeData.GetRecentTradesAsync(symbol, limit)` |
| Index/mark price | `client.FuturesApi.ExchangeData.GetIndexMarkPriceAsync(symbol)`|
| Funding rate | `client.FuturesApi.ExchangeData.GetFundingRateAsync(symbol)`|
| Funding history | `client.FuturesApi.ExchangeData.GetFundingRateHistoryAsync(symbol, ...)` |
| Klines | `client.FuturesApi.ExchangeData.GetKlinesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Index klines | `client.FuturesApi.ExchangeData.GetIndexPriceKlinesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Mark klines | `client.FuturesApi.ExchangeData.GetMarkPriceKlinesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Position tiers | `client.FuturesApi.ExchangeData.GetPositionTiersAsync(symbol, marginMode)` |
| Additional symbol metadata | `client.FuturesApi.ExchangeData.GetSymbolsV3Async()` |

## Futures Account

| Intent | Method |
| --- | --- |
| Futures balances | `client.FuturesApi.Account.GetBalancesAsync(productType)` |
| Margin mode | `client.FuturesApi.Account.GetMarginModeAsync()` |
| Set margin mode | `client.FuturesApi.Account.SetMarginModeAsync(marginMode)` |
| Position mode | `client.FuturesApi.Account.GetPositionModeAsync()` |
| Set position mode | `client.FuturesApi.Account.SetPositionModeAsync(positionMode, multiPositionMode)` |
| Leverage settings | `client.FuturesApi.Account.GetLeverageAsync(symbol, marginMode)` |
| Set leverage | `client.FuturesApi.Account.SetLeverageAsync(symbol, leverage, marginMode, positionSide)` |

## Futures Trading

| Intent | Method |
| --- | --- |
| Positions | `client.FuturesApi.Trading.GetPositionsAsync(symbol)` |
| Place order | `client.FuturesApi.Trading.PlaceOrderAsync(...)` |
| Place batch orders | `client.FuturesApi.Trading.PlaceMultipleOrdersAsync(orders)` |
| Place TP/SL | `client.FuturesApi.Trading.PlaceTpSlOrderAsync(...)` |
| Place trigger order | `client.FuturesApi.Trading.PlaceTriggerOrderAsync(...)` |
| Edit order | `client.FuturesApi.Trading.EditOrderAsync(...)` |
| Cancel order | `client.FuturesApi.Trading.CancelOrderAsync(orderId: ...)` or `client.FuturesApi.Trading.CancelOrderAsync(symbol: ..., clientOrderId: ...)` |
| Open orders | `client.FuturesApi.Trading.GetOpenOrdersAsync(...)` |
| Open TP/SL orders | `client.FuturesApi.Trading.GetOpenTpSlOrdersAsync(...)` |
| Open trigger orders | `client.FuturesApi.Trading.GetOpenTriggerOrdersAsync(...)` |
| Close position | `client.FuturesApi.Trading.ClosePositionAsync(...)` |
| Order details | `client.FuturesApi.Trading.GetOrderAsync(...)` |
| Closed orders | `client.FuturesApi.Trading.GetClosedOrdersAsync(...)` |
| User trades | `client.FuturesApi.Trading.GetUserTradesAsync(...)` |
| Price limits | `client.FuturesApi.Trading.GetPriceLimitsAsync(symbol, side)` |
| Position history | `client.FuturesApi.Trading.GetPositionHistoryAsync(...)` |
| Cancel batch | `client.FuturesApi.Trading.CancelOrdersAsync(...)` |
| Cancel TP/SL batch | `client.FuturesApi.Trading.CancelTpSlOrdersAsync(...)` |
| Cancel trigger order | `client.FuturesApi.Trading.CancelTriggerOrderAsync(...)` |
| TP/SL lookup | `client.FuturesApi.Trading.GetTpSlOrderAsync(...)` |
| Closed TP/SL orders | `client.FuturesApi.Trading.GetClosedTpSlOrdersAsync(...)` |
| Closed trigger orders | `client.FuturesApi.Trading.GetClosedTriggerOrdersAsync(...)` |

## Futures Sockets

| Intent | Method |
| --- | --- |
| Trades | `socketClient.FuturesApi.SubscribeToTradeUpdatesAsync(symbol, ...)` |
| Klines | `socketClient.FuturesApi.SubscribeToKlineUpdatesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Order book | `socketClient.FuturesApi.SubscribeToOrderBookUpdatesAsync(symbol, depth, ...)` |
| Tickers | `socketClient.FuturesApi.SubscribeToTickerUpdatesAsync(symbol, ...)` |
| Funding | `socketClient.FuturesApi.SubscribeToFundingRateUpdatesAsync(symbol, ...)` |
| Private positions | `socketClient.FuturesApi.SubscribeToPositionUpdatesAsync(...)` |
| Private orders | `socketClient.FuturesApi.SubscribeToOrderUpdatesAsync(...)` |
| Private trigger orders | `socketClient.FuturesApi.SubscribeToTriggerOrderUpdatesAsync(...)` |
| Private balances | `socketClient.FuturesApi.SubscribeToBalanceUpdatesAsync(...)` |
| Index-price klines | `socketClient.FuturesApi.SubscribeToIndexPriceKlineUpdatesAsync(...)` |
| Mark-price klines | `socketClient.FuturesApi.SubscribeToMarkPriceKlineUpdatesAsync(...)` |
| Inverse balances | `socketClient.FuturesApi.SubscribeToInverseBalanceUpdatesAsync(...)` |

`socketClient.FuturesApi.SubscribeToFundingRateUpdatesAsync(...)` emits `BloFinFundingUpdate`.

## Spot Sockets

| Intent | Method |
| --- | --- |
| Trades | `socketClient.SpotApi.SubscribeToTradeUpdatesAsync(...)` |
| Klines | `socketClient.SpotApi.SubscribeToKlineUpdatesAsync(...)` |
| Order book | `socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync(...)` |
| Tickers | `socketClient.SpotApi.SubscribeToTickerUpdatesAsync(...)` |
| Private orders | `socketClient.SpotApi.SubscribeToOrderUpdatesAsync(...)` |
| Private balances | `socketClient.SpotApi.SubscribeToBalanceUpdatesAsync(...)` |
| Private algo orders | `socketClient.SpotApi.SubscribeToAlgoOrderUpdatesAsync(...)` |


## Shared API V2

REST surfaces are `client.AccountApi.SharedApi`, `client.SpotApi.SharedApi`, and `client.FuturesApi.SharedApi`. Socket surfaces are `socketClient.SpotApi.SharedApi` and `socketClient.FuturesApi.SharedApi`. The DI aggregate `IBloFinSharedApiClient` exposes `AccountRest`, `SpotRest`, `FuturesRest`, `SpotSocket`, and `FuturesSocket`.

| Surface | Representative capabilities |
| --- | --- |
| Account REST | `IGetDepositHistoryRest`, `IGetWithdrawalHistoryRest` (history reads, no withdrawal placement capability) |
| Spot REST | `IGetSpotSymbolsRest`, `IGetTickerRest`, `IGetBalancesRest`, `ITransferRest`, `IGetTransferHistoryRest`, `IPlaceSpotOrderRest`, `IPlaceMultipleSpotOrdersRest`, `ICancelSpotOrderRest`, order/client-ID lookups and trades |
| Futures REST | `IGetFuturesSymbolsRest`, `IGetTickerRest`, `IGetAllAssetsRest`, `IGetBalancesRest`, `IGetPositionModeRest`, `ISetPositionModeRest`, `IGetLeverageRest`, `ISetLeverageRest`, `IPlaceFuturesOrderRest`, `IPlaceMultipleFuturesOrdersRest`, `IEditFuturesOrderRest`, `IGetPositionsRest`, `ICloseFullPositionRest`, TP/SL and trigger-order capabilities |
| Spot socket | `ISubscribeTradesSocket`, `ISubscribeTickerSocket`, `ISubscribeKlinesSocket`, `ISubscribeOrderBookSocket`, `ISubscribeIncrementalOrderBookSocket`, `ISubscribeBalancesSocket`, `ISubscribeSpotOrdersSocket` |
| Futures socket | `ISubscribeTradesSocket`, `ISubscribeTickerSocket`, `ISubscribeKlinesSocket`, `ISubscribeOrderBookSocket`, `ISubscribeBookTickerSocket`, `ISubscribeFundingInfoSocket`, `ISubscribeBalancesSocket`, `ISubscribeFuturesOrdersSocket`, `ISubscribePositionsSocket` |

For dynamic selection, call the aggregate's `GetCapability` with a typed descriptor such as `SharedCapabilities.Tickers.GetTicker.Rest` and the required trading mode. Check for a missing match before accessing `Capability` and `Options`. Use `GetCapabilities` to enumerate implementations and `Discover()` for supported-operation metadata. A descriptor does not guarantee that every surface implements the operation. With multiple exchanges registered in DI, use `IEnumerable<TCapability>` and select by exchange and supported trading mode.

### Symbols and quantity representations

Use `IGetSpotSymbolsRest.GetSpotSymbolsAsync(new GetSymbolsRequest())` or `IGetFuturesSymbolsRest.GetFuturesSymbolsAsync(new GetSymbolsRequest())`. After a successful request, the corresponding `SpotSymbolCatalog` or `FuturesSymbolCatalog` supplies cached normalized metadata. Requests support base/quote asset type and subtype filters; results include `DisplayName`, trading constraints, and asset classifications (`Crypto`, `Fiat`, `TradFi`; applicable `StableCoin`, `Equity`, `Commodity` subtypes).

Futures symbol enrichment uses V3 metadata for contract constraints and asset classifications. The native `GetSymbolsV3Async` endpoint is undocumented and may change; use the standard symbol request when extra metadata is unnecessary.

Shared futures quantities use `SharedOrderQuantity.QuantityInContracts`; futures order books report `SharedQuantityType.Contracts`. Use `ContractSize` when converting contracts to asset exposure. Spot books use `SharedQuantityType.BaseAsset`; spot order quantity representations depend on order type and side, so inspect the capability options. Shared futures placement requires margin mode and does not set leverage inline; use the separate leverage capability when needed.

Shared REST returns `HttpResult<T>` / `HttpResult`, subscriptions return `WebSocketResult<UpdateSubscription>`, and non-I/O symbol/cache helpers can return `ExchangeCallResult<T>`. Check result success before accessing data.

## Options, environments, and helpers

Per-API configuration uses `FuturesOptions` and `SpotOptions`. Built-in environments are `BloFinEnvironment.Live` and `BloFinEnvironment.Test`; assign the intended environment on client options. `CreateCustom` takes a name, REST address, and socket address.

`BloFinOrderBookFactory` exposes `Futures`, `Spot`, `CreateFutures`, and `CreateSpot` for maintained local books. `BloFinTrackerFactory` provides futures and spot user-data trackers; generic kline and trade trackers are futures-only.

Checkout-only: [public interfaces](../../../../BloFin.Net/Interfaces/Clients/), [order books](../../../../BloFin.Net/SymbolOrderBooks/), and [tracker factory](../../../../BloFin.Net/BloFinTrackerFactory.cs).
