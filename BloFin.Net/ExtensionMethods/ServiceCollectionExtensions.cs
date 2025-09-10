using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using BloFin.Net;
using BloFin.Net.Clients;
using BloFin.Net.Interfaces;
using BloFin.Net.Interfaces.Clients;
using BloFin.Net.Objects.Options;
using BloFin.Net.SymbolOrderBooks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the IBloFinRestClient and IBloFinSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddBloFin(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new BloFinOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            configuration.Bind(options);

            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            var restEnvName = options.Rest.Environment?.Name ?? options.Environment?.Name ?? BloFinEnvironment.Live.Name;
            var socketEnvName = options.Socket.Environment?.Name ?? options.Environment?.Name ?? BloFinEnvironment.Live.Name;
            options.Rest.Environment = BloFinEnvironment.GetEnvironmentByName(restEnvName) ?? options.Rest.Environment!;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = BloFinEnvironment.GetEnvironmentByName(socketEnvName) ?? options.Socket.Environment!;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;


            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddBloFinCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the IBloFinRestClient and IBloFinSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the BloFin services</param>
        /// <returns></returns>
        public static IServiceCollection AddBloFin(
            this IServiceCollection services,
            Action<BloFinOptions>? optionsDelegate = null)
        {
            var options = new BloFinOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            optionsDelegate?.Invoke(options);
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment = options.Rest.Environment ?? options.Environment ?? BloFinEnvironment.Live;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = options.Socket.Environment ?? options.Environment ?? BloFinEnvironment.Live;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddBloFinCore(services, options.SocketClientLifeTime);
        }

        private static IServiceCollection AddBloFinCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<IBloFinRestClient, BloFinRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<BloFinRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new BloFinRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<BloFinRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<BloFinRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options.Proxy, options.HttpKeepAliveInterval);
            });
            services.Add(new ServiceDescriptor(typeof(IBloFinSocketClient), x => { return new BloFinSocketClient(x.GetRequiredService<IOptions<BloFinSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<ICryptoRestClient, CryptoRestClient>();
            services.AddSingleton<ICryptoSocketClient, CryptoSocketClient>();
            services.AddTransient<IBloFinOrderBookFactory, BloFinOrderBookFactory>();
            services.AddTransient<IBloFinTrackerFactory, BloFinTrackerFactory>();
            services.AddSingleton<IBloFinUserClientProvider, BloFinUserClientProvider>(x =>
                new BloFinUserClientProvider(
                    x.GetRequiredService<HttpClient>(),
                    x.GetRequiredService<ILoggerFactory>(),
                    x.GetRequiredService<IOptions<BloFinRestOptions>>(),
                    x.GetRequiredService<IOptions<BloFinSocketOptions>>()));

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IBloFinRestClient>().ExchangeApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IBloFinSocketClient>().ExchangeApi.SharedClient);

            return services;
        }
    }
}
