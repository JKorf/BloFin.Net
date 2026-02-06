using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;

namespace BloFin.Net.Interfaces
{
    /// <summary>
    /// Tracker factory
    /// </summary>
    public interface IBloFinTrackerFactory : ITrackerFactory
    {
        /// <summary>
        /// Create a new futures user data tracker
        /// </summary>
        /// <param name="userIdentifier">User identifier</param>
        /// <param name="config">Configuration</param>
        /// <param name="credentials">Credentials</param>
        /// <param name="environment">Environment</param>
        IUserFuturesDataTracker CreateUserFuturesDataTracker(string userIdentifier, ApiCredentials credentials, FuturesUserDataTrackerConfig? config = null, BloFinEnvironment? environment = null);
        /// <summary>
        /// Create a new futures user data tracker
        /// </summary>
        /// <param name="config">Configuration</param>
        IUserFuturesDataTracker CreateUserFuturesDataTracker(FuturesUserDataTrackerConfig? config = null);
    }
}
