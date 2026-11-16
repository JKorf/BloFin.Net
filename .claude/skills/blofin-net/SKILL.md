---
name: blofin-net
description: Implement or review BloFin.Net spot and futures REST calls, account and order workflows, WebSocket subscriptions, passphrase authentication, Shared API integration, and dependency injection in .NET applications.
---
# BloFin.Net

Install with `dotnet add package BloFin.Net`.

- Reuse `BloFinRestClient` and `BloFinSocketClient`, or resolve them through dependency injection. REST exposes `AccountApi`, `SpotApi`, and `FuturesApi`; sockets expose `SpotApi` and `FuturesApi`.
- Private operations use `BloFinCredentials(key, secret, passphrase)`. Public market data needs no credentials. Configure private socket credentials on the socket client.
- Native spot and futures symbols use hyphen-separated instrument IDs such as `ETH-USDT`. Discover instruments through the relevant `ExchangeData.GetSymbolsAsync`.
- Futures quantities are contracts, including Shared API quantities. Do not treat them as spot base-asset sizes.
- REST returns `HttpResult<T>` or `HttpResult`; subscriptions return `WebSocketResult<UpdateSubscription>`. Check `Success` before reading `Data`, and handle `Error` on failure. For batches, inspect each nested `CallResult` after the outer result succeeds.
- Keep the concrete socket client for subscription cleanup with `UnsubscribeAsync(subscription.Data)`.
- Use each API root's `SharedApi` for Shared API V2. Depend on the needed capability interface and check support and trading mode when selecting an implementation dynamically.

## References

- Read [API surfaces](references/api-surfaces.md) for intent-to-method routing, Shared API capabilities, catalogs, quantities, environments, and tracking helpers.
- Read [usage](references/usage.md) for authentication, account reads, trading, subscription lifecycle, Shared API V2, and DI snippets.
- Read [operational considerations](references/safety.md) for market-order quantities, position modes, batch outcomes, and submission recovery.

Checkout-only: [repository instructions](../../../AGENTS.md) and [complete examples](../../../Examples/ai-friendly/).
