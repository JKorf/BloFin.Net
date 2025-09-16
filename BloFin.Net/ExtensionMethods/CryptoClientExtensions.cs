using BloFin.Net.Clients;
using BloFin.Net.Interfaces.Clients;

namespace CryptoExchange.Net.Interfaces
{
    /// <summary>
    /// Extensions for the ICryptoRestClient and ICryptoSocketClient interfaces
    /// </summary>
    public static class CryptoClientExtensions
    {
        /// <summary>
        /// Get the BloFin REST Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IBloFinRestClient BloFin(this ICryptoRestClient baseClient) => baseClient.TryGet<IBloFinRestClient>(() => new BloFinRestClient());

        /// <summary>
        /// Get the BloFin Websocket Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IBloFinSocketClient BloFin(this ICryptoSocketClient baseClient) => baseClient.TryGet<IBloFinSocketClient>(() => new BloFinSocketClient());
    }
}
