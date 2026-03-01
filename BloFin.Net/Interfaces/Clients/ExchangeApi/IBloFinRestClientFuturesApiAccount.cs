using BloFin.Net.Enums;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// BloFin Exchange account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IBloFinRestClientFuturesApiAccount
    {
        /// <summary>
        /// Get balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-futures-account-balance" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/account/balance
        /// </para>
        /// </summary>
        /// <param name="productType">Product type</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinFuturesBalances>> GetBalancesAsync(ProductType? productType = null, CancellationToken ct = default);

        /// <summary>
        /// Get margin mode config
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-margin-mode" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/account/margin-mode
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinMarginMode>> GetMarginModeAsync(CancellationToken ct = default);

        /// <summary>
        /// Set margin mode
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#set-margin-mode" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/account/set-margin-mode
        /// </para>
        /// </summary>
        /// <param name="marginMode">New margin mode</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinMarginMode>> SetMarginModeAsync(MarginMode marginMode, CancellationToken ct = default);

        /// <summary>
        /// Get position mode config
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-position-mode" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/account/position-mode
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinPositionMode>> GetPositionModeAsync(CancellationToken ct = default);

        /// <summary>
        /// Set position mode
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#set-position-mode" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/account/set-position-mode
        /// </para>
        /// </summary>
        /// <param name="positionMode">Position mode</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinPositionMode>> SetPositionModeAsync(PositionMode positionMode, CancellationToken ct = default);

        /// <summary>
        /// Get leverage settings
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-multiple-leverage" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/account/batch-leverage-info
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="marginMode">Margin mode</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinLeverage[]>> GetLeverageAsync(string symbol, MarginMode marginMode, CancellationToken ct = default);

        /// <summary>
        /// Set leverage
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#set-leverage" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/account/set-leverage
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="leverage">Leverage</param>
        /// <param name="marginMode">The margin mode</param>
        /// <param name="positionSide">Position side</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinLeverage>> SetLeverageAsync(string symbol, decimal leverage, MarginMode marginMode, PositionSide? positionSide = null, CancellationToken ct = default);

    }
}
