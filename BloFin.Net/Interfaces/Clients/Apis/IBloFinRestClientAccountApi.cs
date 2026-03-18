using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.AccountApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Interfaces.Clients.Apis
{
    /// <summary>
    /// BloFin account API endpoints
    /// </summary>
    public interface IBloFinRestClientAccountApi : IRestApiClient<BloFinCredentials>, IDisposable
    {

        /// <summary>
        /// Get account balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-instruments" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/asset/balances
        /// </para>
        /// </summary>
        /// <param name="accountType">["<c>accountType</c>"] Account type</param>
        /// <param name="asset">["<c>currency</c>"] Filter by asset</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinBalance[]>> GetAccountBalancesAsync(AccountType accountType, string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Transfer asset between account types
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#funds-transfer" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/asset/transfer
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] The asset</param>
        /// <param name="fromAccount">["<c>fromAccount</c>"] From account</param>
        /// <param name="toAccount">["<c>toAccount</c>"] To account</param>
        /// <param name="quantity">["<c>amount</c>"] Quantity to transfer</param>
        /// <param name="clientId">["<c>clientId</c>"] Client transfer id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTransferResult>> TransferAsync(string asset, AccountType fromAccount, AccountType toAccount, decimal quantity, string? clientId = null, CancellationToken ct = default);

        /// <summary>
        /// Get account config
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-account-config" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/account/config
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinAccountConfig>> GetAccountConfigAsync(CancellationToken ct = default);

        /// <summary>
        /// Get API key info
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-api-key-info" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/user/query-apikey
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinApiKey>> GetApiKeyInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Get transfer history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-funds-transfer-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/asset/bills
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Filter by asset, for example `ETH`</param>
        /// <param name="fromAccount">["<c>fromAccount</c>"] Filter by from account</param>
        /// <param name="toAccount">["<c>toAccount</c>"] Filter by to account</param>
        /// <param name="startTime">["<c>before</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>after</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinTransfer[]>> GetTransferHistoryAsync(string? asset = null, AccountType? fromAccount = null, AccountType? toAccount = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get withdrawal history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-withdraw-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/asset/withdrawal-history
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Filter by asset, for example `ETH`</param>
        /// <param name="withdrawId">["<c>withdrawId</c>"] Filter by withdrawal id</param>
        /// <param name="transactionId">["<c>txId</c>"] Filter by transaction id</param>
        /// <param name="status">["<c>state</c>"] Filter by status</param>
        /// <param name="startTime">["<c>before</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>after</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinWithdrawal[]>> GetWithdrawalHistoryAsync(string? asset = null, string? withdrawId = null, string? transactionId = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/index.html#get-deposit-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/asset/deposit-history
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Filter by asset, for example `ETH`</param>
        /// <param name="depositId">["<c>depositId</c>"] Filter by deposit id</param>
        /// <param name="transactionId">["<c>txId</c>"] Filter by transaction id</param>
        /// <param name="status">["<c>state</c>"] Filter by status</param>
        /// <param name="startTime">["<c>before</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>after</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinDeposit[]>> GetDepositHistoryAsync(string? asset = null, string? depositId = null, string? transactionId = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IBloFinRestClientAccountApiShared SharedClient { get; }
    }
}
