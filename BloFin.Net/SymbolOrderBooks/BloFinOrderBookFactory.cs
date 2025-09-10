using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using BloFin.Net.Interfaces;
using BloFin.Net.Interfaces.Clients;
using BloFin.Net.Objects.Options;

namespace BloFin.Net.SymbolOrderBooks
{
    /// <summary>
    /// BloFin order book factory
    /// </summary>
    public class BloFinOrderBookFactory : IBloFinOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BloFinOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;            
            
            Exchange = new OrderBookFactory<BloFinOrderBookOptions>(Create, Create);
        }

        
         /// <inheritdoc />
        public IOrderBookFactory<BloFinOrderBookOptions> Exchange { get; }


        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<BloFinOrderBookOptions>? options = null)
        {
            var symbolName = symbol.GetSymbol(BloFinExchange.FormatSymbol);

            return Create(symbolName, options);
        }

        
         /// <inheritdoc />
        public ISymbolOrderBook Create(string symbol, Action<BloFinOrderBookOptions>? options = null)
            => new BloFinExchangeSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<IBloFinRestClient>(),
                                                          _serviceProvider.GetRequiredService<IBloFinSocketClient>());


    }
}
