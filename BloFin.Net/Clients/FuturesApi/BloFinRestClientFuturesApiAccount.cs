using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class BloFinRestClientFuturesApiAccount : IBloFinRestClientFuturesApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BloFinRestClientFuturesApi _baseClient;

        internal BloFinRestClientFuturesApiAccount(BloFinRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinBalance[]>> GetBalancesAsync(AccountType accountType, string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("accountType", accountType);
            parameters.AddOptional("currency", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/asset/balances", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await _baseClient.SendAsync<BloFinBalance[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinTransferResult>> TransferAsync(string asset, AccountType fromAccount, AccountType toAccount, decimal quantity, string? clientId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("currency", asset);
            parameters.AddEnum("toAccount", toAccount);
            parameters.AddEnum("fromAccount", fromAccount);
            parameters.AddString("amount", quantity);
            parameters.AddOptional("clientId", clientId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/asset/transfer", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await _baseClient.SendAsync<BloFinTransferResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Config

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinAccountConfig>> GetAccountConfigAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/account/config", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await _baseClient.SendAsync<BloFinAccountConfig>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Config

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinApiKey>> GetApiKeyInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/user/query-apikey", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await _baseClient.SendAsync<BloFinApiKey>(request, null, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
