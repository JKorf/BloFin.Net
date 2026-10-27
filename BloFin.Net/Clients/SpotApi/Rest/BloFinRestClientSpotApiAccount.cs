using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.SpotApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class BloFinRestClientSpotApiAccount : IBloFinRestClientSpotApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BloFinRestClientSpotApi _baseClient;

        /// <summary>
        /// ctor
        /// </summary>
        internal BloFinRestClientSpotApiAccount(BloFinRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<BloFinBalance[]>> GetBalancesAsync(
            AccountType accountType,
            string? asset = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("accountType", accountType);
            parameters.Add("currency", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/asset/balances", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinBalance[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<HttpResult<BloFinTransferResult>> TransferAsync(
            string asset,
            AccountType fromAccount,
            AccountType toAccount,
            decimal quantity,
            string? clientId = null,
            string? subAccountId = null,
            bool? mainToSubAccount = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("currency", asset);
            parameters.Add("fromAccount", fromAccount);
            parameters.Add("toAccount", toAccount);
            parameters.Add("amount", quantity);
            parameters.Add("clientId", clientId);
            parameters.Add("subAccount", subAccountId);
            parameters.Add("mainToSubAccount", mainToSubAccount);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/asset/transfer", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTransferResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Transfer History

        /// <inheritdoc />
        public async Task<HttpResult<BloFinTransfer[]>> GetTransferHistoryAsync(
            string? asset = null,
            AccountType? fromAccount = null,
            AccountType? toAccount = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("currency", asset);
            parameters.Add("fromAccount", fromAccount);
            parameters.Add("toAccount", toAccount);
            // Before/After filter works inverted on the server
            parameters.Add("after", endTime);
            parameters.Add("before", startTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/asset/bills", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTransfer[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion
    }
}
