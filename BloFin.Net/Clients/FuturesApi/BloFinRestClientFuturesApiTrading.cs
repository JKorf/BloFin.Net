using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BloFin.Net.Interfaces.Clients.FuturesApi;

namespace BloFin.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class BloFinRestClientFuturesApiTrading : IBloFinRestClientFuturesApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BloFinRestClientFuturesApi _baseClient;
        private readonly ILogger _logger;

        internal BloFinRestClientFuturesApiTrading(ILogger logger, BloFinRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }
    }
}
