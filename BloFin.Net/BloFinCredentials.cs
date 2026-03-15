using CryptoExchange.Net.Authentication;
using System;

namespace BloFin.Net
{
    /// <summary>
    /// BloFin credentials
    /// </summary>
    public class BloFinCredentials : ApiCredentials
    {
        /// <summary>
        /// </summary>
        [Obsolete("Parameterless constructor is only for deserialization purposes and should not be used directly. Use parameterized constructor instead.")]
        public BloFinCredentials() { }

        /// <summary>
        /// Create credentials using an HMAC key, secret and passphrase.
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="secret">The API secret</param>
        /// <param name="pass">Passphrase</param>
        public BloFinCredentials(string apiKey, string secret, string pass) : this(new HMACCredential(apiKey, secret, pass)) { }

        /// <summary>
        /// Create BloFin credentials using HMAC credentials
        /// </summary>
        /// <param name="credential">The HMAC credentials</param>
        public BloFinCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
        public override ApiCredentials Copy() => new BloFinCredentials { CredentialPairs = CredentialPairs };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
