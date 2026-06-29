# BloFin.Net AI-Friendly Examples

These examples are intentionally small console programs that AI assistants can copy from safely. They are compiled by `BloFin.Net.UnitTests/Documentation/AiExampleCompileTests.cs`.

## Files

| File | Demonstrates |
| --- | --- |
| `01-futures-market-and-account.cs` | Public futures market data plus general/futures balances |
| `02-futures-trading.cs` | Futures limit order flow, open orders, cancel and positions |
| `03-websocket.cs` | Public and private futures websocket subscriptions |
| `04-shared-client.cs` | Shared-client access, capability discovery and native BloFin calls |
| `05-error-handling.cs` | Reusable `HttpResult` and `WebSocketResult` handling helpers |

## BloFin Shape To Remember

Use:

```csharp
restClient.AccountApi
restClient.FuturesApi.ExchangeData
restClient.FuturesApi.Account
restClient.FuturesApi.Trading
socketClient.FuturesApi
```

Do not use `ExchangeApi`, `SpotApi`, `UsdFuturesApi`, `FuturesApiV2`, `SpotApiV3`, `CoinFuturesApi`, or `PerpetualFuturesApi`.

BloFin credentials require key, secret and passphrase:

```csharp
new BloFinCredentials("API_KEY", "API_SECRET", "PASSPHRASE")
```

Use BloFin futures symbols such as `ETH-USDT`. Use `KlineInterval` for candles.
