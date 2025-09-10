using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using BloFin.Net.Interfaces.Clients.ExchangeApi;

namespace BloFin.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BloFinRestClientExchangeApiTrading : IBloFinRestClientExchangeApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BloFinRestClientExchangeApi _baseClient;
        private readonly ILogger _logger;

        internal BloFinRestClientExchangeApiTrading(ILogger logger, BloFinRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }
    }
}
