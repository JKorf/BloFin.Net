using CryptoExchange.Net.Objects;
using BloFin.Net.Clients.ExchangeApi;
using BloFin.Net.Interfaces.Clients.ExchangeApi;

namespace BloFin.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BloFinRestClientExchangeApiAccount : IBloFinRestClientExchangeApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BloFinRestClientExchangeApi _baseClient;

        internal BloFinRestClientExchangeApiAccount(BloFinRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
        }
    }
}
