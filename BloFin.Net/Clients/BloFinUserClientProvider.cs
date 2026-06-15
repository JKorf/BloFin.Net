using BloFin.Net.Interfaces.Clients;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace BloFin.Net.Clients
{
    /// <inheritdoc />
    public class BloFinUserClientProvider :  UserClientProvider<
        IBloFinRestClient,
        IBloFinSocketClient,
        BloFinRestOptions,
        BloFinSocketOptions,
        BloFinCredentials,
        BloFinEnvironment
        >, IBloFinUserClientProvider
    {
        /// <inheritdoc />
        public override string ExchangeName => BloFinExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public BloFinUserClientProvider(Action<BloFinOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }
        
        /// <summary>
        /// ctor
        /// </summary>
        public BloFinUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<BloFinRestOptions> restOptions,
            IOptions<BloFinSocketOptions> socketOptions): base(httpClient, loggerFactory, restOptions, socketOptions)
        {
        }

        /// <inheritdoc />
        protected override IBloFinRestClient ConstructRestClient(
            HttpClient client,
            ILoggerFactory? loggerFactory, 
            IOptions<BloFinRestOptions> options) => new BloFinRestClient(client, loggerFactory, options);

        /// <inheritdoc />
        protected override IBloFinSocketClient ConstructSocketClient(
            ILoggerFactory? loggerFactory,
            IOptions<BloFinSocketOptions> options) => new BloFinSocketClient(options, loggerFactory);
    }
}
