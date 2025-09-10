using CryptoExchange.Net.Objects;
using BloFin.Net.Objects;

namespace BloFin.Net
{
    /// <summary>
    /// BloFin environments
    /// </summary>
    public class BloFinEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Rest API address
        /// </summary>
        public string RestClientAddress { get; }

        /// <summary>
        /// Socket API address
        /// </summary>
        public string SocketClientAddress { get; }

        internal BloFinEnvironment(
            string name,
            string restAddress,
            string streamAddress) :
            base(name)
        {
            RestClientAddress = restAddress;
            SocketClientAddress = streamAddress;
        }

        /// <summary>
        /// ctor for DI, use <see cref="CreateCustom"/> for creating a custom environment
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public BloFinEnvironment() : base(TradeEnvironmentNames.Live)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        { }

        /// <summary>
        /// Get the BloFin environment by name
        /// </summary>
        public static BloFinEnvironment? GetEnvironmentByName(string? name)
         => name switch
         {
             TradeEnvironmentNames.Live => Live,
             TradeEnvironmentNames.Testnet => Test,
             "" => Live,
             null => Live,
             _ => default
         };

        /// <summary>
        /// Available environment names
        /// </summary>
        /// <returns></returns>
        public static string[] All => [Live.Name, Test.Name];

        /// <summary>
        /// Live environment
        /// </summary>
        public static BloFinEnvironment Live { get; }
            = new BloFinEnvironment(TradeEnvironmentNames.Live,
                                     BloFinApiAddresses.Default.RestClientAddress,
                                     BloFinApiAddresses.Default.SocketClientAddress);

        /// <summary>
        /// Test environment
        /// </summary>
        public static BloFinEnvironment Test { get; }
            = new BloFinEnvironment(TradeEnvironmentNames.Testnet,
                                     BloFinApiAddresses.Test.RestClientAddress,
                                     BloFinApiAddresses.Test.SocketClientAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spotRestAddress"></param>
        /// <param name="spotSocketStreamsAddress"></param>
        /// <returns></returns>
        public static BloFinEnvironment CreateCustom(
                        string name,
                        string spotRestAddress,
                        string spotSocketStreamsAddress)
            => new BloFinEnvironment(name, spotRestAddress, spotSocketStreamsAddress);
    }
}
