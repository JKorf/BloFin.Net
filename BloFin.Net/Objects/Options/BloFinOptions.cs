using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;

namespace BloFin.Net.Objects.Options
{
    /// <summary>
    /// BloFin options
    /// </summary>
    public class BloFinOptions : LibraryOptions<BloFinRestOptions, BloFinSocketOptions, ApiCredentials, BloFinEnvironment>
    {
    }
}
