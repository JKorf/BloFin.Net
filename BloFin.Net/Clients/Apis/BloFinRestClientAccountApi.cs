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

        internal BloFinRestClientAccountApi(ILogger logger, HttpClient? httpClient, BloFinRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinBalance[]>> GetAccountBalancesAsync(AccountType accountType, string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("accountType", accountType);
            parameters.AddOptional("currency", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/asset/balances", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await SendAsync<BloFinBalance[]>(request, parameters, ct).ConfigureAwait(false);
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
            return await SendAsync<BloFinTransferResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Config

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinAccountConfig>> GetAccountConfigAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/account/config", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await SendAsync<BloFinAccountConfig>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Config

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinApiKey>> GetApiKeyInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/user/query-apikey", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            return await SendAsync<BloFinApiKey>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Transfer History

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinTransfer[]>> GetTransferHistoryAsync(string? asset = null, AccountType? fromAccount = null, AccountType? toAccount = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("currency", asset);
            parameters.AddOptionalEnum("fromAccount", fromAccount);
            parameters.AddOptionalEnum("toAccount", toAccount);
            // Before/After filter works inverted on the server
            parameters.AddOptionalMillisecondsString("after", endTime);
            parameters.AddOptionalMillisecondsString("before", startTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/asset/bills", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await SendAsync<BloFinTransfer[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Withdrawal History

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinWithdrawal[]>> GetWithdrawalHistoryAsync(string? asset = null, string? withdrawId = null, string? transactionId = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("currency", asset);
            parameters.AddOptional("withdrawId", withdrawId);
            parameters.AddOptional("txId", transactionId);
            parameters.AddOptionalEnum("state", status);
            // Before/After filter works inverted on the server
            parameters.AddOptionalMillisecondsString("after", endTime);
            parameters.AddOptionalMillisecondsString("before", startTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/asset/withdrawal-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await SendAsync<BloFinWithdrawal[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Deposit History

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinDeposit[]>> GetDepositHistoryAsync(string? asset = null, string? depositId = null, string? transactionId = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("currency", asset);
            parameters.AddOptional("depositId", depositId);
            parameters.AddOptional("txId", transactionId);
            parameters.AddOptionalEnum("state", status);
            // Before/After filter works inverted on the server
            parameters.AddOptionalMillisecondsString("after", endTime);
            parameters.AddOptionalMillisecondsString("before", startTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/asset/deposit-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await SendAsync<BloFinDeposit[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        /// <inheritdoc />
        public IBloFinRestClientAccountApiShared SharedClient => this;
    }
}
