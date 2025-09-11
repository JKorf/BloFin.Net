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
        /// Get account balances
        /// <para><a href="https://docs.blofin.com/index.html#get-instruments" /></para>
        /// </summary>
        /// <param name="accountType">Account type</param>
        /// <param name="asset">Filter by asset</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<BloFinBalance[]>> GetBalancesAsync(AccountType accountType, string? asset = null, CancellationToken ct = default);

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
    }
}
