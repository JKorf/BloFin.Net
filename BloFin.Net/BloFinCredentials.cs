using CryptoExchange.Net.Authentication;

namespace BloFin.Net
{
    /// <summary>
    /// BloFin credentials
    /// </summary>
    public class BloFinCredentials : ApiCredentials
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="secret">The API secret</param>
        /// <param name="pass">Passphrase</param>
        public BloFinCredentials(string apiKey, string secret, string pass) : this(new HMACCredential(apiKey, secret, pass)) { }
       
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="credential">The HMAC credentials</param>
        public BloFinCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new BloFinCredentials(Hmac!);
    }
}
