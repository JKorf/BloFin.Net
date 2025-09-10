using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using BloFin.Net.Objects.Options;

namespace BloFin.Net.Interfaces
{
    /// <summary>
    /// BloFin local order book factory
    /// </summary>
    public interface IBloFinOrderBookFactory
    {
        
        /// <summary>
        /// Exchange order book factory methods
        /// </summary>
        IOrderBookFactory<BloFinOrderBookOptions> Exchange { get; }


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
        ISymbolOrderBook Create(string symbol, Action<BloFinOrderBookOptions>? options = null);

    }
}