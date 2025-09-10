using CryptoExchange.Net.Objects.Options;

namespace BloFin.Net.Objects.Options
{
    /// <summary>
    /// Options for the BloFinRestClient
    /// </summary>
    public class BloFinRestOptions : RestExchangeOptions<BloFinEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static BloFinRestOptions Default { get; set; } = new BloFinRestOptions()
        {
            Environment = BloFinEnvironment.Live,
            AutoTimestamp = true
        };

        /// <summary>
        /// ctor
        /// </summary>
        public BloFinRestOptions()
        {
            Default?.Set(this);
        }

        
         /// <summary>
        /// Exchange API options
        /// </summary>
        public RestApiOptions ExchangeOptions { get; private set; } = new RestApiOptions();


        internal BloFinRestOptions Set(BloFinRestOptions targetOptions)
        {
            targetOptions = base.Set<BloFinRestOptions>(targetOptions);
            
            targetOptions.ExchangeOptions = ExchangeOptions.Set(targetOptions.ExchangeOptions);

            return targetOptions;
        }
    }
}
