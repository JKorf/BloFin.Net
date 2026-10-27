using CryptoExchange.Net.Objects.Options;

namespace BloFin.Net.Objects.Options
{
    /// <summary>
    /// Options for the BloFinRestClient
    /// </summary>
    public class BloFinRestOptions : RestExchangeOptions<BloFinEnvironment, BloFinCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static BloFinRestOptions Default { get; set; } = new BloFinRestOptions()
        {
            Environment = BloFinEnvironment.Live
        };

        /// <summary>
        /// ctor
        /// </summary>
        public BloFinRestOptions()
        {
            Default?.Set(this);
        }

        /// <summary>
        /// Broker id
        /// </summary>
        public string? BrokerId { get; set; }
        
        /// <summary>
        /// Futures API options
        /// </summary>
        public RestApiOptions FuturesOptions { get; private set; } = new RestApiOptions();

        /// <summary>
        /// Spot API options
        /// </summary>
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions();

        internal BloFinRestOptions Set(BloFinRestOptions targetOptions)
        {
            targetOptions = base.Set<BloFinRestOptions>(targetOptions);
            targetOptions.BrokerId = BrokerId;
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);

            return targetOptions;
        }
    }
}
