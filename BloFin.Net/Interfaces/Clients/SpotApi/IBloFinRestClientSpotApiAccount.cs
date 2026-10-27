using BloFin.Net.Enums;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BloFin account endpoints
    /// </summary>
    public interface IBloFinRestClientSpotApiAccount
    {
        /// <summary>
        /// Get account balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-balance" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/asset/balances<br />
        /// </para>
        /// </summary>
        /// <param name="accountType">["<c>accountType</c>"] Account type, for unified account use Futures</param>
        /// <param name="asset">["<c>currency</c>"] Filter by asset, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinBalance[]>> GetBalancesAsync(
            AccountType accountType,
            string? asset = null,
            CancellationToken ct = default);

        /// <summary>
        /// Transfer between accounts
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#funds-transfer" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/asset/transfer<br />
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] The asset, for example `ETH`</param>
        /// <param name="fromAccount">["<c>fromAccount</c>"] From account type</param>
        /// <param name="toAccount">["<c>toAccount</c>"] To account type</param>
        /// <param name="quantity">["<c>amount</c>"] Quantity to transfer</param>
        /// <param name="clientId">["<c>clientId</c>"] Client id</param>
        /// <param name="subAccountId">["<c>subAccount</c>"] Sub account id</param>
        /// <param name="mainToSubAccount">["<c>mainToSubAccount</c>"] Main to sub account</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinTransferResult>> TransferAsync(
            string asset,
            AccountType fromAccount,
            AccountType toAccount,
            decimal quantity,
            string? clientId = null,
            string? subAccountId = null,
            bool? mainToSubAccount = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get transfer history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.blofin.com/spot.html#get-funds-transfer-history" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/asset/bills<br />
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>currency</c>"] Filter by asset, for example `ETH`</param>
        /// <param name="fromAccount">["<c>fromAccount</c>"] Filter by target account</param>
        /// <param name="toAccount">["<c>toAccount</c>"] Filter by source account</param>
        /// <param name="startTime">["<c>after</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>before</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinTransfer[]>> GetTransferHistoryAsync(
            string? asset = null,
            AccountType? fromAccount = null,
            AccountType? toAccount = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

    }
}
