using BloFin.Net.Interfaces.Clients.SpotApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
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

        /// <inheritdoc />
        public async Task<HttpResult<BloFinSpotSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/market/instruments", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            return await _baseClient.SendAsync<BloFinSpotSymbol[]>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<HttpResult<BloFinSpotTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BloFinExchange._parameterSerializationSettings);
            parameters.Add("instType", "SPOT");
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/spot/market/tickers", BloFinExchange.RateLimiter.BloFinRest, 1, false);
            return await _baseClient.SendAsync<BloFinSpotTicker[]>(request, parameters, ct).ConfigureAwait(false);
        }
    }
}
