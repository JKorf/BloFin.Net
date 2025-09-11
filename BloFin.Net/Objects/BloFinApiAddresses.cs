namespace BloFin.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class BloFinApiAddresses
    {
        /// <summary>
        /// The address used by the BloFinRestClient for the API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the BloFinSocketClient for the websocket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the BloFin API
        /// </summary>
        public static BloFinApiAddresses Default = new BloFinApiAddresses
        {
            RestClientAddress = "https://openapi.blofin.com",
            SocketClientAddress = "wss://openapi.blofin.com"
        };

        /// <summary>
        /// The default addresses to connect to the BloFin API
        /// </summary>
        public static BloFinApiAddresses Test = new BloFinApiAddresses
        {
            RestClientAddress = "https://demo-trading-openapi.blofin.com",
            SocketClientAddress = "wss://demo-trading-openapi.blofin.com"
        };
    }
}
