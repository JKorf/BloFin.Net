using BloFin.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.Logging;

namespace BloFin.Net
{
    /// <inheritdoc />
    public class BloFinUserFuturesDataTracker : UserFuturesDataTracker
    {
        /// <inheritdoc />
        protected override bool WebsocketPositionUpdatesAreFullSnapshots => false;

        /// <summary>
        /// ctor
        /// </summary>
        public BloFinUserFuturesDataTracker(
            ILogger<BloFinUserFuturesDataTracker> logger,
            IBloFinRestClient restClient,
            IBloFinSocketClient socketClient,
            string? userIdentifier,
            FuturesUserDataTrackerConfig? config) : base(logger,
                restClient.FuturesApi.SharedClient,
                null,
                restClient.FuturesApi.SharedClient,
                socketClient.FuturesApi.SharedClient,
                restClient.FuturesApi.SharedClient,
                socketClient.FuturesApi.SharedClient,
                null,
                socketClient.FuturesApi.SharedClient,
                userIdentifier,
                config ?? new FuturesUserDataTrackerConfig())
        {
        }
    }
}
