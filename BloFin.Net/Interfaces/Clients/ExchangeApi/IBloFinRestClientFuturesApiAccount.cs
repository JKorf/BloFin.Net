using BloFin.Net.Enums;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
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
        /// Get account balances
        /// <para><a href="https://docs.blofin.com/index.html#get-instruments" /></para>
        /// </summary>
        /// <param name="accountType">Account type</param>
        /// <param name="asset">Filter by asset</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinBalance[]>> GetAccountBalancesAsync(AccountType accountType, string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Transfer asset between account types
        /// <para><a href="https://docs.blofin.com/index.html#funds-transfer" /></para>
        /// </summary>
        /// <param name="asset">The asset</param>
        /// <param name="fromAccount">From account</param>
        /// <param name="toAccount">To account</param>
        /// <param name="quantity">Quantity to transfer</param>
        /// <param name="clientId">Client transfer id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTransferResult>> TransferAsync(string asset, AccountType fromAccount, AccountType toAccount, decimal quantity, string? clientId = null, CancellationToken ct = default);

        /// <summary>
        /// Get account config
        /// <para><a href="https://docs.blofin.com/index.html#get-account-config" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinAccountConfig>> GetAccountConfigAsync(CancellationToken ct = default);

        /// <summary>
        /// Get API key info
        /// <para><a href="https://docs.blofin.com/index.html#get-api-key-info" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinApiKey>> GetApiKeyInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Get transfer history
        /// <para><a href="https://docs.blofin.com/index.html#get-funds-transfer-history" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset, for example `ETH`</param>
        /// <param name="fromAccount">Filter by from account</param>
        /// <param name="toAccount">Filter by to account</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTransfer[]>> GetTransferHistoryAsync(string? asset = null, AccountType? fromAccount = null, AccountType? toAccount = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get withdrawal history
        /// <para><a href="https://docs.blofin.com/index.html#get-withdraw-history" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset, for example `ETH`</param>
        /// <param name="withdrawId">Filter by withdrawal id</param>
        /// <param name="transactionId">Filter by transaction id</param>
        /// <param name="status">Filter by status</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinWithdrawal[]>> GetWithdrawalHistoryAsync(string? asset = null, string? withdrawId = null, string? transactionId = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit history
        /// <para><a href="https://docs.blofin.com/index.html#get-deposit-history" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset, for example `ETH`</param>
        /// <param name="depositId">Filter by deposit id</param>
        /// <param name="transactionId">Filter by transaction id</param>
        /// <param name="status">Filter by status</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinDeposit[]>> GetDepositHistoryAsync(string? asset = null, string? depositId = null, string? transactionId = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get balances
        /// <para><a href="https://docs.blofin.com/index.html#get-futures-account-balance" /></para>
        /// </summary>
        /// <param name="productType">Product type</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinFuturesBalances>> GetBalancesAsync(ProductType? productType = null, CancellationToken ct = default);

        /// <summary>
        /// Get margin mode config
        /// <para><a href="https://docs.blofin.com/index.html#get-margin-mode" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinMarginMode>> GetMarginModeAsync(CancellationToken ct = default);

        /// <summary>
        /// Set margin mode
        /// <para><a href="https://docs.blofin.com/index.html#set-margin-mode" /></para>
        /// </summary>
        /// <param name="marginMode">New margin mode</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinMarginMode>> SetMarginModeAsync(MarginMode marginMode, CancellationToken ct = default);

        /// <summary>
        /// Get position mode config
        /// <para><a href="https://docs.blofin.com/index.html#get-position-mode" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinPositionMode>> GetPositionModeAsync(CancellationToken ct = default);

        /// <summary>
        /// Set position mode
        /// <para><a href="https://docs.blofin.com/index.html#set-position-mode" /></para>
        /// </summary>
        /// <param name="positionMode">Position mode</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinPositionMode>> SetPositionModeAsync(PositionMode positionMode, CancellationToken ct = default);

        /// <summary>
        /// Get leverage settings
        /// <para><a href="https://docs.blofin.com/index.html#get-multiple-leverage" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="marginMode">Margin mode</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinLeverage[]>> GetLeverageAsync(string symbol, MarginMode marginMode, CancellationToken ct = default);

        /// <summary>
        /// Set leverage
        /// <para><a href="https://docs.blofin.com/index.html#set-leverage" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="leverage">Leverage</param>
        /// <param name="marginMode">The margin mode</param>
        /// <param name="positionSide">Position side</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinLeverage>> SetLeverageAsync(string symbol, decimal leverage, MarginMode marginMode, PositionSide? positionSide = null, CancellationToken ct = default);

    }
}
