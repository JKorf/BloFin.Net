using BloFin.Net.Interfaces.Clients;
using BloFin.Net.Interfaces.Clients.AccountApi;
using BloFin.Net.Interfaces.Clients.FuturesApi;

namespace BloFin.Net.Clients
{
    /// <inheritdoc />
    public class BloFinSharedApiClient : IBloFinSharedApiClient
    {
        /// <inheritdoc />
        public IBloFinRestClientAccountSharedApi AccountRest { get; }
        /// <inheritdoc />
        public IBloFinRestClientFuturesSharedApi FuturesRest { get; }
        /// <inheritdoc />
        public IBloFinSocketClientFuturesSharedApi FuturesSocket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public BloFinSharedApiClient(
            IBloFinRestClient restClient,
            IBloFinSocketClient socketClient)
        {
            AccountRest = restClient.AccountApi.SharedApi;
            FuturesRest = restClient.FuturesApi.SharedApi;
            FuturesSocket = socketClient.FuturesApi.SharedApi;
        }
    }
}
