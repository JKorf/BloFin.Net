using BloFin.Net.Interfaces.Clients;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.Logging;

namespace BloFin.Net
{
    /// <inheritdoc />
    public class BloFinUserSpotDataTracker : UserSpotDataTracker
    {
        /// <summary>
        /// ctor
        /// </summary>
        public BloFinUserSpotDataTracker(
            ILogger<BloFinUserSpotDataTracker> logger,
            IBloFinRestClient restClient,
            IBloFinSocketClient socketClient,
            string? userIdentifier,
            SpotUserDataTrackerConfig? config) : base(logger,
                restClient.SpotApi.SharedApi,
                restClient.SpotApi.SharedApi,
                socketClient.SpotApi.SharedApi,

                restClient.SpotApi.SharedApi,
                restClient.SpotApi.SharedApi,
                socketClient.SpotApi.SharedApi,

                restClient.SpotApi.SharedApi,
                null,

                userIdentifier,
                config ?? new SpotUserDataTrackerConfig())
        {
        }
    }

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
                restClient.FuturesApi.SharedApi,
                restClient.FuturesApi.SharedApi,
                socketClient.FuturesApi.SharedApi,

                restClient.FuturesApi.SharedApi,
                restClient.FuturesApi.SharedApi,
                socketClient.FuturesApi.SharedApi,

                restClient.FuturesApi.SharedApi,
                null,

                restClient.FuturesApi.SharedApi,
                socketClient.FuturesApi.SharedApi,
                userIdentifier,
                config ?? new FuturesUserDataTrackerConfig())
        {
        }
    }
}
