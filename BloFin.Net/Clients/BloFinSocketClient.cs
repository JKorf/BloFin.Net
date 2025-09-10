using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using BloFin.Net.Interfaces.Clients;
using BloFin.Net.Objects.Options;
using BloFin.Net.Interfaces.Clients.ExchangeApi;
using BloFin.Net.Clients.ExchangeApi;

namespace BloFin.Net.Clients
{
    /// <inheritdoc cref="IBloFinSocketClient" />
    public class BloFinSocketClient : BaseSocketClient, IBloFinSocketClient
    {
        #region fields
        #endregion

        #region Api clients

        
         /// <inheritdoc />
        public IBloFinSocketClientExchangeApi ExchangeApi { get; }


        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of BloFinSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BloFinSocketClient(Action<BloFinSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of BloFinSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public BloFinSocketClient(IOptions<BloFinSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "BloFin")
        {
            Initialize(options.Value);

            ExchangeApi = AddApiClient(new BloFinSocketClientExchangeApi(_logger, options.Value));
        }
        #endregion

        /// <inheritdoc />
        public void SetOptions(UpdateOptions options)
        {
            ExchangeApi.SetOptions(options);
        }

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BloFinSocketOptions> optionsDelegate)
        {
            BloFinSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {            
            ExchangeApi.SetApiCredentials(credentials);
        }
    }
}
