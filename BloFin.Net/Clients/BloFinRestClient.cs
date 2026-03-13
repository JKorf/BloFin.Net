using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using CryptoExchange.Net.Authentication;
using BloFin.Net.Interfaces.Clients;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Options;
using CryptoExchange.Net.Objects.Options;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Clients.FuturesApi;
using BloFin.Net.Interfaces.Clients.Apis;
using BloFin.Net.Clients.Apis;

namespace BloFin.Net.Clients
{
    /// <inheritdoc cref="IBloFinRestClient" />
    internal class BloFinRestClient : BaseRestClient<BloFinEnvironment, BloFinCredentials>, IBloFinRestClient
    {
        #region Api clients
                
         /// <inheritdoc />
        public IBloFinRestClientAccountApi AccountApi { get; }
         /// <inheritdoc />
        public IBloFinRestClientFuturesApi FuturesApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the BloFinRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BloFinRestClient(Action<BloFinRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the BloFinRestClient using provided options
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public BloFinRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<BloFinRestOptions> options) : base(loggerFactory, "BloFin")
        {
            Initialize(options.Value);
                        
            AccountApi = AddApiClient(new BloFinRestClientAccountApi(_logger, httpClient, options.Value));
            FuturesApi = AddApiClient(new BloFinRestClientFuturesApi(_logger, httpClient, options.Value));
        }

        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BloFinRestOptions> optionsDelegate)
        {
            BloFinRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
    }
}
