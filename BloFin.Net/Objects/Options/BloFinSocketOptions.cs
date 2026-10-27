using CryptoExchange.Net.Objects.Options;

namespace BloFin.Net.Objects.Options
{
    /// <summary>
    /// Options for the BloFinSocketClient
    /// </summary>
    public class BloFinSocketOptions : SocketExchangeOptions<BloFinEnvironment, BloFinCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static BloFinSocketOptions Default { get; set; } = new BloFinSocketOptions()
        {
            Environment = BloFinEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// ctor
        /// </summary>
        public BloFinSocketOptions()
        {
            Default?.Set(this);
        }
        
        /// <summary>
        /// Futures API options
        /// </summary>
        public SocketApiOptions FuturesOptions { get; private set; } = new SocketApiOptions();

        /// <summary>
        /// Spot API options
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions();

        internal BloFinSocketOptions Set(BloFinSocketOptions targetOptions)
        {
            targetOptions = base.Set<BloFinSocketOptions>(targetOptions);            
            targetOptions.FuturesOptions = FuturesOptions.Set(targetOptions.FuturesOptions);
            targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
            return targetOptions;
        }
    }
}
