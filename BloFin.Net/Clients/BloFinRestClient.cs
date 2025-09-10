using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using CryptoExchange.Net.Authentication;
using BloFin.Net.Interfaces.Clients;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Options;
using CryptoExchange.Net.Objects.Options;
using BloFin.Net.Interfaces.Clients.ExchangeApi;
using BloFin.Net.Clients.ExchangeApi;

namespace BloFin.Net.Clients
{
    /// <inheritdoc cref="IBloFinRestClient" />
    public class BloFinRestClient : BaseRestClient, IBloFinRestClient
    {
        #region Api clients
                
         /// <inheritdoc />
        public IBloFinRestClientExchangeApi ExchangeApi { get; }

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
                        
            ExchangeApi = AddApiClient(new BloFinRestClientExchangeApi(this, _logger, httpClient, options.Value));
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
        public static void SetDefaultOptions(Action<BloFinRestOptions> optionsDelegate)
        {
            BloFinRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            ExchangeApi.SetApiCredentials(credentials);
        }
    }
}
