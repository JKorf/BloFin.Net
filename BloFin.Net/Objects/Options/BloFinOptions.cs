using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace BloFin.Net.Objects.Options
{
    /// <summary>
    /// BloFin options
    /// </summary>
    public class BloFinOptions : LibraryOptions<BloFinRestOptions, BloFinSocketOptions, BloFinCredentials, BloFinEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
    }
}
