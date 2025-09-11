using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
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
        public async Task<WebCallResult<BloFinSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("isntId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/instruments", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            return await _baseClient.SendAsync<BloFinSymbol[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/tickers", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddOptional("size", depth);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/books", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinOrderBook[]>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BloFinOrderBook>(result.Data?.Single());
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/trades", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Index Mark Price

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinMarkIndexPrice>> GetIndexMarkPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/mark-price", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinMarkIndexPrice[]>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BloFinMarkIndexPrice>(result.Data?.Single());
        }

        #endregion

        #region Get Funding Rate

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinFundingRate>> GetFundingRateAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/funding-rate", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinFundingRate[]>(request, parameters, ct).ConfigureAwait(false);
            return result.As<BloFinFundingRate>(result.Data?.Single());
        }

        #endregion

        #region Get Funding Rate History

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinFundingRate[]>> GetFundingRateHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            // NOTE; the before parameter actually acts as startTime
            parameters.AddOptionalMillisecondsString("before", startTime);
            parameters.AddOptionalMillisecondsString("after", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/funding-rate-history", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinFundingRate[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("bar", interval);
            // NOTE; the before parameter actually acts as startTime
            parameters.AddOptionalMillisecondsString("before", startTime);
            parameters.AddOptionalMillisecondsString("after", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/candles", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Index Price Klines

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinIndexMarkKline[]>> GetIndexPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("bar", interval);
            // NOTE; the before parameter actually acts as startTime
            parameters.AddOptionalMillisecondsString("before", startTime);
            parameters.AddOptionalMillisecondsString("after", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/index-candles", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinIndexMarkKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Mark Price Klines

        /// <inheritdoc />
        public async Task<WebCallResult<BloFinIndexMarkKline[]>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("bar", interval);
            // NOTE; the before parameter actually acts as startTime
            parameters.AddOptionalMillisecondsString("before", startTime);
            parameters.AddOptionalMillisecondsString("after", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/mark-price-candles", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinIndexMarkKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion
    }
}
