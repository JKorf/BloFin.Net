using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.SpotApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class BloFinRestClientSpotApiExchangeData : IBloFinRestClientSpotApiExchangeData
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BloFinRestClientSpotApi _baseClient;

        /// <summary>
        /// Create the spot market data client
        /// </summary>
        internal BloFinRestClientSpotApiExchangeData(BloFinRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<BloFinSpotSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/market/instruments", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            return await _baseClient.SendAsync<BloFinSpotSymbol[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<HttpResult<BloFinSpotTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/market/tickers", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            return await _baseClient.SendAsync<BloFinSpotTicker[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<BloFinOrderBook>> GetOrderBookAsync(
            string symbol,
            int depth,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("size", depth);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/market/books", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinOrderBook[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BloFinOrderBook>(result);

            var book = result.Data.FirstOrDefault();
            if (book == null)
                return HttpResult.Fail<BloFinOrderBook>(result, new ServerError(ErrorType.Unknown, "No order book data returned"));

            return HttpResult.Ok(result, book);
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<HttpResult<BloFinSpotTrade[]>> GetRecentTradesAsync(
            string symbol,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/market/trades", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinSpotTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<BloFinKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            parameters.Add("bar", interval);
            // Before/After filter works inverted on the server
            parameters.Add("after", endTime);
            parameters.Add("before", startTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/market/candles", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            var result = await _baseClient.SendAsync<BloFinKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
