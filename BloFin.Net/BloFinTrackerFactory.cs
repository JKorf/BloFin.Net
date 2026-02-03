using BloFin.Net.Clients;
using BloFin.Net.Interfaces;
using BloFin.Net.Interfaces.Clients;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;

namespace BloFin.Net
{
    /// <inheritdoc />
    public class BloFinTrackerFactory : IBloFinTrackerFactory
    {
        private readonly IServiceProvider? _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        public BloFinTrackerFactory()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BloFinTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                return false;

            var client = (_serviceProvider?.GetRequiredService<IBloFinSocketClient>() ?? new BloFinSocketClient());
            return client.FuturesApi.SharedClient.SubscribeKlineOptions.IsSupported(interval);
        }

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol) => true;

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBloFinRestClient>() ?? new BloFinRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBloFinSocketClient>() ?? new BloFinSocketClient();

            if (symbol.TradingMode == TradingMode.Spot)
                throw new InvalidOperationException("Spot not supported");
            
            var sharedRestClient = restClient.FuturesApi.SharedClient;
            var sharedSocketClient = socketClient.FuturesApi.SharedClient;

            return new KlineTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                sharedRestClient,
                sharedSocketClient,
                symbol,
                interval,
                limit,
                period
                );
        }
        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBloFinRestClient>() ?? new BloFinRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBloFinSocketClient>() ?? new BloFinSocketClient();

            if (symbol.TradingMode == TradingMode.Spot)            
                throw new InvalidOperationException("Spot not supported");

            var sharedRestClient = restClient.FuturesApi.SharedClient;
            var sharedSocketClient = socketClient.FuturesApi.SharedClient;

            return new TradeTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                sharedRestClient,
                null,
                sharedSocketClient,
                symbol,
                limit,
                period
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserFuturesDataTracker(FuturesUserDataTrackerConfig config)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBloFinRestClient>() ?? new BloFinRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBloFinSocketClient>() ?? new BloFinSocketClient();
            return new BloFinUserFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BloFinUserFuturesDataTracker>>() ?? new NullLogger<BloFinUserFuturesDataTracker>(),
                restClient,
                socketClient,
                null,
                config
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserFuturesDataTracker(string userIdentifier, FuturesUserDataTrackerConfig config, ApiCredentials credentials, BloFinEnvironment? environment = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IBloFinUserClientProvider>() ?? new BloFinUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new BloFinUserFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BloFinUserFuturesDataTracker>>() ?? new NullLogger<BloFinUserFuturesDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config
                );
        }
    }
}
