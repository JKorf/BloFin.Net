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
        public async Task<HttpResult<BloFinFuturesBalances>> GetBalancesAsync(ProductType? productType = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("productType", productType);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/account/balance", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinFuturesBalances>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Margin Mode

        /// <inheritdoc />
        public async Task<HttpResult<BloFinMarginMode>> GetMarginModeAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/account/margin-mode", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinMarginMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Margin Mode

        /// <inheritdoc />
        public async Task<HttpResult<BloFinMarginMode>> SetMarginModeAsync(MarginMode marginMode, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("marginMode", marginMode);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/account/set-margin-mode", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinMarginMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Position Mode

        /// <inheritdoc />
        public async Task<HttpResult<BloFinPositionMode>> GetPositionModeAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/account/position-mode", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinPositionMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Position Mode

        /// <inheritdoc />
        public async Task<HttpResult<BloFinPositionMode>> SetPositionModeAsync(PositionMode positionMode, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("positionMode", positionMode);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/account/set-position-mode", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinPositionMode>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Leverage

        /// <inheritdoc />
        public async Task<HttpResult<BloFinLeverage[]>> GetLeverageAsync(string symbol, MarginMode marginMode, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("marginMode", marginMode);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/account/batch-leverage-info", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinLeverage[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Leverage

        /// <inheritdoc />
        public async Task<HttpResult<BloFinLeverage>> SetLeverageAsync(string symbol, decimal leverage, MarginMode marginMode, PositionSide? positionSide = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("leverage", leverage);
            parameters.Add("marginMode", marginMode);
            parameters.Add("positionSide", positionSide);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/account/set-leverage", BloFinExchange.RateLimiter.BloFinRest, 1, true);
            var result = await _baseClient.SendAsync<BloFinLeverage>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
