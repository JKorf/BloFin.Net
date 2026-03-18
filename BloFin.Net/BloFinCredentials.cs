using CryptoExchange.Net.Authentication;

namespace BloFin.Net
{
    /// <summary>
    /// BloFin API credentials
    /// </summary>
    public class BloFinCredentials : HMACCredential
    {
        /// <summary>
        /// Create new credentials providing only credentials in HMAC format
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        /// <param name="pass">Passphrase</param>
        public BloFinCredentials(string key, string secret, string pass) : base(key, secret, pass)
        {
        }
    }
}
