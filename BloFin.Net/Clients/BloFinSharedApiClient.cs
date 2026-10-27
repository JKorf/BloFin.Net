using BloFin.Net.Interfaces.Clients;
using BloFin.Net.Interfaces.Clients.AccountApi;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Interfaces.Clients.SpotApi;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Options;

namespace BloFin.Net.Clients
{
    /// <inheritdoc />
    public class BloFinSharedApiClient : SharedApiClientBase, IBloFinSharedApiClient
    {
        /// <inheritdoc />
        public IBloFinRestClientAccountSharedApi AccountRest { get; }
        /// <inheritdoc />
        public IBloFinRestClientFuturesSharedApi FuturesRest { get; }
        /// <inheritdoc />
        public IBloFinSocketClientFuturesSharedApi FuturesSocket { get; }
        /// <inheritdoc />
        public IBloFinRestClientSpotSharedApi SpotRest { get; }
        /// <inheritdoc />
        public IBloFinSocketClientSpotSharedApi SpotSocket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public BloFinSharedApiClient(
            IBloFinRestClient restClient,
            IBloFinSocketClient socketClient,
            IOptions<BloFinOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                  restClient.AccountApi.SharedApi,
                  restClient.FuturesApi.SharedApi,
                  restClient.SpotApi.SharedApi,
                  socketClient.FuturesApi.SharedApi,
                  socketClient.SpotApi.SharedApi)
        {
            AccountRest = restClient.AccountApi.SharedApi;
            FuturesRest = restClient.FuturesApi.SharedApi;
            FuturesSocket = socketClient.FuturesApi.SharedApi;
            SpotRest = restClient.SpotApi.SharedApi;
            SpotSocket = socketClient.SpotApi.SharedApi;
        }
    }
}
