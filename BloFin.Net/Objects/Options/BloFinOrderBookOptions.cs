using CryptoExchange.Net.Objects.Options;
using System;

namespace BloFin.Net.Objects.Options
{
    /// <summary>
    /// Options for the BloFin SymbolOrderBook
    /// </summary>
    public class BloFinOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static BloFinOrderBookOptions Default { get; set; } = new BloFinOrderBookOptions();

        /// <summary>
        /// The top amount of results to keep in sync. 5 or 400
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        internal BloFinOrderBookOptions Copy()
        {
            var result = Copy<BloFinOrderBookOptions>();
            result.Limit = Limit;
            result.InitialDataTimeout = InitialDataTimeout;
            return result;
        }
    }
}
