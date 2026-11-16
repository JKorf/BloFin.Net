# Operational considerations

## Credentials and environments

Private operations require the API key, secret, and passphrase. Keep all three outside source and grant permissions appropriate to the selected calls. Repository onboarding recommends creating the key as `Connect to Third-Party Application` with `JKorf` selected as the application name.

Clients default to `BloFinEnvironment.Live`. Choose `BloFinEnvironment.Test` explicitly for the test environment and use credentials appropriate to the selected environment. A custom endpoint configuration does not itself imply simulated execution.

## Spot execution

Spot symbol metadata supplies `MinQuantity`, `LotSize`, `TickSize`, and maximum limit/market quantities. Market-order quantity semantics depend on `AssetType`; choose the intended target currency rather than assuming every quantity is in the base asset.

Limit orders can execute immediately if their price crosses the book. Cancellation can race with a fill: check cancellation results and reconcile order state and trades rather than assuming no execution occurred.

## Futures position semantics

Futures quantities are contract counts. Convert to asset exposure using symbol contract metadata; shared quantities use `QuantityInContracts`. Select margin mode and position side for the actual account position mode: `PositionSide.Net` for one-way behavior and `Long`/`Short` for hedge-mode intent as applicable.

Leverage, margin mode, position mode, close-position, TP/SL, and trigger-order operations can change exposure. Query current settings before changing them. Shared futures order placement requires margin mode and does not accept leverage inline; use the separate leverage operation when needed. Distinguish regular, TP/SL, and trigger-order lifecycles when querying or canceling orders.

## Batch outcomes and ambiguous submissions

Batch placement and cancellation can return `HttpResult<CallResult<T>[]>`. Check the outer request, then inspect `Success` and `Error` for every item. Retry only the items whose state has been reconciled; outer success does not prove the full batch succeeded.

A timeout or lost response can occur after BloFin accepted a submission. Query the order by the supported order/client identifier, or inspect the relevant transfer/account state, before deciding whether to resubmit. A client-order ID helps correlation but does not by itself prove resubmission is idempotent. Transient transport errors do not make account-changing calls safe to repeat; handle public-read retries separately.

## Account scope and transfers

General account balances use `AccountType`; futures trading balances use `ProductType`. Keep funding, spot, and futures account scopes distinct. Transfers move balances between accounts: verify asset, direction, quantity, and the resulting transfer status. The account Shared API provides deposit and withdrawal history reads, not withdrawal placement.
