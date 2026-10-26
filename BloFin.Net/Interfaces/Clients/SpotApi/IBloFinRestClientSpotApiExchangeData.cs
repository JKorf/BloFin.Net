using BloFin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// BloFin public spot market data
    /// </summary>
    public interface IBloFinRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Get spot instruments and their trading constraints.
        /// <para><a href="https://docs.blofin.com/spot.html#get-instruments" /></para>
        /// </summary>
        /// <param name="symbol">Optional symbol filter, for example BTC-USDT</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinSpotSymbol[]>> GetSymbolsAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get spot prices and 24-hour base-asset volume.
        /// <para><a href="https://docs.blofin.com/spot.html#get-tickers" /></para>
        /// </summary>
        /// <param name="symbol">Optional symbol filter, for example BTC-USDT</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BloFinSpotTicker[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default);
    }
}
