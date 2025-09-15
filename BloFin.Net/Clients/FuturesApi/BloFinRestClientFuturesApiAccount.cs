using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System.Net.Http;
using System;
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

#warning These endpoints are not futures specific, should be in a more general topic

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinBalance[]>> GetAccountBalancesAsync(AccountType accountType, string? asset = null, CancellationToken ct = default)
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

        #region Get Transfer History

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinTransfer[]>> GetTransferHistoryAsync(string? asset = null, AccountType? fromAccount = null, AccountType? toAccount = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("currency", asset);
            parameters.AddOptionalEnum("fromAccount", fromAccount);
            parameters.AddOptionalEnum("toAccount", toAccount);
            parameters.AddOptionalMillisecondsString("after", startTime);
            parameters.AddOptionalMillisecondsString("before", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/asset/bills", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinTransfer[]>(request, parameters, ct).ConfigureAwait(false);
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
            parameters.AddOptionalMillisecondsString("after", startTime);
            parameters.AddOptionalMillisecondsString("before", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/asset/withdrawal-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinWithdrawal[]>(request, parameters, ct).ConfigureAwait(false);
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
            parameters.AddOptionalMillisecondsString("after", startTime);
            parameters.AddOptionalMillisecondsString("before", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/asset/deposit-history", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinDeposit[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinFuturesBalances>> GetBalancesAsync(ProductType? productType = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalEnum("productType", productType);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/account/balance", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinFuturesBalances>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Margin Mode

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinMarginMode>> GetMarginModeAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/account/margin-mode", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinMarginMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Margin Mode

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinMarginMode>> SetMarginModeAsync(MarginMode marginMode, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("marginMode", marginMode);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/account/set-margin-mode", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinMarginMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Position Mode

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinPositionMode>> GetPositionModeAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/account/position-mode", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinPositionMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Position Mode

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinPositionMode>> SetPositionModeAsync(PositionMode positionMode, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("positionMode", positionMode);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/account/set-position-mode", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinPositionMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Leverage

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinLeverage[]>> GetLeverageAsync(string symbol, MarginMode marginMode, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("marginMode", marginMode);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/account/batch-leverage-info", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinLeverage[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Leverage

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinLeverage>> SetLeverageAsync(string symbol, decimal leverage, MarginMode marginMode, PositionSide? positionSide = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddString("leverage", leverage);
            parameters.AddEnum("marginMode", marginMode);
            parameters.AddOptionalEnum("positionSide", positionSide);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/api/v1/account/set-leverage", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinLeverage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
