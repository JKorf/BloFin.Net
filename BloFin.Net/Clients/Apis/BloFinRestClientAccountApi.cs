using BloFin.Net.Clients.MessageHandlers;
using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.AccountApi;
using BloFin.Net.Interfaces.Clients.Apis;
using BloFin.Net.Objects.Models;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.Apis
{
    internal partial class BloFinRestClientAccountApi : BloFinRestClientApi, IBloFinRestClientAccountApi
    {
        protected override ErrorMapping ErrorMapping => BloFinErrors.Errors;
        protected override IRestMessageHandler MessageHandler { get; } = new BloFinRestMessageHandler(BloFinErrors.Errors);

        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal BloFinRestClientAccountApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, BloFinRestOptions options)
            : base(loggerFactory, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<BloFinBalance[]>> GetAccountBalancesAsync(AccountType accountType, string? asset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("accountType", accountType);
            parameters.Add("currency", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "/api/v1/asset/balances", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await SendAsync<BloFinBalance[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<HttpResult<BloFinTransferResult>> TransferAsync(string asset, AccountType fromAccount, AccountType toAccount, decimal quantity, string? clientId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("currency", asset);
            parameters.Add("toAccount", toAccount);
            parameters.Add("fromAccount", fromAccount);
            parameters.Add("amount", quantity);
            parameters.Add("clientId", clientId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, BaseAddress, "/api/v1/asset/transfer", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await SendAsync<BloFinTransferResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Config

        /// <inheritdoc />
        public async Task<HttpResult<BloFinAccountConfig>> GetAccountConfigAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "/api/v1/account/config", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await SendAsync<BloFinAccountConfig>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Config

        /// <inheritdoc />
        public async Task<HttpResult<BloFinApiKey>> GetApiKeyInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "/api/v1/user/query-apikey", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await SendAsync<BloFinApiKey>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Transfer History

        /// <inheritdoc />
        public async Task<HttpResult<BloFinTransfer[]>> GetTransferHistoryAsync(string? asset = null, AccountType? fromAccount = null, AccountType? toAccount = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("currency", asset);
            parameters.Add("fromAccount", fromAccount);
            parameters.Add("toAccount", toAccount);
            // Before/After filter works inverted on the server
            parameters.Add("after", endTime);
            parameters.Add("before", startTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "/api/v1/asset/bills", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await SendAsync<BloFinTransfer[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Withdrawal History

        /// <inheritdoc />
        public async Task<HttpResult<BloFinWithdrawal[]>> GetWithdrawalHistoryAsync(string? asset = null, string? withdrawId = null, string? transactionId = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("currency", asset);
            parameters.Add("withdrawId", withdrawId);
            parameters.Add("txId", transactionId);
            parameters.Add("state", status);
            // Before/After filter works inverted on the server
            parameters.Add("after", endTime);
            parameters.Add("before", startTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "/api/v1/asset/withdrawal-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await SendAsync<BloFinWithdrawal[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Deposit History

        /// <inheritdoc />
        public async Task<HttpResult<BloFinDeposit[]>> GetDepositHistoryAsync(string? asset = null, string? depositId = null, string? transactionId = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("currency", asset);
            parameters.Add("depositId", depositId);
            parameters.Add("txId", transactionId);
            parameters.Add("state", status);
            // Before/After filter works inverted on the server
            parameters.Add("after", endTime);
            parameters.Add("before", startTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, BaseAddress, "/api/v1/asset/deposit-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await SendAsync<BloFinDeposit[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        /// <inheritdoc />
        public IBloFinRestClientAccountApiShared SharedClient => this;
    }
}
