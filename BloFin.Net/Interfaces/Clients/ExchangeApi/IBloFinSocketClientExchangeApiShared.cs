using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloFin.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Shared interface for Exchange socket API usage
    /// </summary>
    public interface IBloFinSocketClientFuturesApiShared :
        IBookTickerSocketClient,
        IKlineSocketClient,
        IOrderBookSocketClient,
        ITickerSocketClient,
        ITradeSocketClient,
        IBalanceSocketClient,
        IFuturesOrderSocketClient,
        IPositionSocketClient
    {
    }
}
