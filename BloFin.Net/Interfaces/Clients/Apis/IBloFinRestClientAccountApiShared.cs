using CryptoExchange.Net.SharedApis;

namespace BloFin.Net.Interfaces.Clients.AccountApi
{
    /// <summary>
    /// Shared interface for account rest API usage
    /// </summary>
    public interface IBloFinRestClientAccountApiShared :
        IWithdrawalRestClient,
        IDepositRestClient
    {
    }

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IBloFinRestClientAccountSharedApi :
        IGetWithdrawalHistoryRest,
        IGetDepositHistoryRest
    { }
}
