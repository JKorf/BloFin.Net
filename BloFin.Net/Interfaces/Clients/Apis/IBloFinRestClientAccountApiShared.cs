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
}
