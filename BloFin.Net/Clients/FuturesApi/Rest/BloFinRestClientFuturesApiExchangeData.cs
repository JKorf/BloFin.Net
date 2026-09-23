using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Objects.Models;
using System.Linq;
using BloFin.Net.Enums;

namespace BloFin.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class BloFinRestClientFuturesApiExchangeData : IBloFinRestClientFuturesApiExchangeData
    {
        private readonly BloFinRestClientFuturesApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal BloFinRestClientFuturesApiExchangeData(ILogger logger, BloFinRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<BloFinSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/instruments", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            return await _baseClient.SendAsync<BloFinSymbol[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Symbols V3

        /// <inheritdoc />
        public async Task<HttpResult<BloFinSymbolV3[]>> GetSymbolsV3Async(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "https://blofin.com", "uapi/v3/basic/symbols", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinSymbolV3[]>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<HttpResult<BloFinTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/tickers", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("size", depth);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/books", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinOrderBook[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BloFinOrderBook>(result);

            return HttpResult.Ok(result, result.Data.Single());
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<HttpResult<BloFinTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/trades", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Index Mark Price

        /// <inheritdoc />
        public async Task<HttpResult<BloFinMarkIndexPrice>> GetIndexMarkPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/mark-price", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinMarkIndexPrice[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BloFinMarkIndexPrice>(result);

            return HttpResult.Ok(result, result.Data.Single());
        }

        #endregion

        #region Get Funding Rate

        /// <inheritdoc />
        public async Task<HttpResult<BloFinFundingRate>> GetFundingRateAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/funding-rate", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinFundingRate[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BloFinFundingRate>(result);

            return HttpResult.Ok(result, result.Data.Single());
        }

        #endregion

        #region Get Funding Rate History

        /// <inheritdoc />
        public async Task<HttpResult<BloFinFundingRate[]>> GetFundingRateHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            // NOTE; the before parameter actually acts as startTime
            parameters.Add("before", startTime);
            parameters.Add("after", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/funding-rate-history", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinFundingRate[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<BloFinKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("bar", interval);
            // NOTE; the before parameter actually acts as startTime
            parameters.Add("before", startTime);
            parameters.Add("after", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/candles", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Index Price Klines

        /// <inheritdoc />
        public async Task<HttpResult<BloFinIndexMarkKline[]>> GetIndexPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("bar", interval);
            // NOTE; the before parameter actually acts as startTime
            parameters.Add("before", startTime);
            parameters.Add("after", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/index-candles", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinIndexMarkKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Mark Price Klines

        /// <inheritdoc />
        public async Task<HttpResult<BloFinIndexMarkKline[]>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("bar", interval);
            // NOTE; the before parameter actually acts as startTime
            parameters.Add("before", startTime);
            parameters.Add("after", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/mark-price-candles", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinIndexMarkKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Position Tiers

        /// <inheritdoc />
        public async Task<HttpResult<BloFinPositionTier[]>> GetPositionTiersAsync(string symbol, MarginMode marginMode, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("marginMode", marginMode);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/position-tiers", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinPositionTier[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion
    }
}
