using CryptoExchange.Net.Objects.Options;

namespace BloFin.Net.Objects.Options
{
    /// <summary>
    /// Options for the BloFinSocketClient
    /// </summary>
    public class BloFinSocketOptions : SocketExchangeOptions<BloFinEnvironment>
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
        /// Exchange API options
        /// </summary>
        public SocketApiOptions ExchangeOptions { get; private set; } = new SocketApiOptions();


        internal BloFinSocketOptions Set(BloFinSocketOptions targetOptions)
        {
            targetOptions = base.Set<BloFinSocketOptions>(targetOptions);
            
            targetOptions.ExchangeOptions = ExchangeOptions.Set(targetOptions.ExchangeOptions);

            return targetOptions;
        }
    }
}
