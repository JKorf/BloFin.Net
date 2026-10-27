using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using BloFin.Net.Objects.Options;

namespace BloFin.Net.Interfaces
{
    /// <summary>
    /// BloFin local order book factory
    /// </summary>
    public interface IBloFinOrderBookFactory : IExchangeService
    {
        /// <summary>
        /// Spot order book factory methods
        /// </summary>
        IOrderBookFactory<BloFinOrderBookOptions> Spot { get; }
        /// <summary>
        /// Futures order book factory methods
        /// </summary>
        IOrderBookFactory<BloFinOrderBookOptions> Futures { get; }

        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<BloFinOrderBookOptions>? options = null);
                
        /// <summary>
        /// Create a new local order book instance
        /// </summary>
        ISymbolOrderBook CreateFutures(string symbol, Action<BloFinOrderBookOptions>? options = null);

        /// <summary>
        /// Create a new local order book instance
        /// </summary>
        ISymbolOrderBook CreateSpot(string symbol, Action<BloFinOrderBookOptions>? options = null);
    }
}