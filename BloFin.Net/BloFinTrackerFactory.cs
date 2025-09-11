using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using BloFin.Net.Interfaces;
using BloFin.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using BloFin.Net.Clients;

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
    }
}
