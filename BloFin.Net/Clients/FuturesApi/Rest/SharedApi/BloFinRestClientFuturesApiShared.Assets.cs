using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.FuturesApi
{
    internal partial class BloFinRestClientFuturesSharedApi
    {
        #region Get All Assets

        async Task<IExchangeCallResult<SharedAsset[]>> IGetAllAssets.GetAllAssetsAsync(GetAssetsRequest request, CancellationToken ct)
            => await ((IGetAllAssetsRest)this).GetAllAssetsAsync(request, ct).ConfigureAwait(false);

        public GetAllAssetsOptions GetAllAssetsOptions { get; } = new GetAllAssetsOptions(_exchangeName, false);
        public async Task<HttpResult<SharedAsset[]>> GetAllAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = GetAllAssetsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset[]>(Exchange, validationError);

            var result = await _api.Account.GetAssetsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedAsset[]>(result);

            var results = result.Data.GroupBy(x => x.Asset);

            return HttpResult.Ok(result, results.Select(x =>
                new SharedAsset(x.Key)
                {
                    Networks = x.Select(x => new SharedAssetNetwork(x.Network)
                    {
                        DepositEnabled = x.DepositAvailable,
                        MinConfirmations = x.DepositConfirmation,
                        MinWithdrawQuantity = x.MinWithdraw,
                        WithdrawEnabled = x.WithdrawAvailable,
                        WithdrawFee = x.WithdrawFee
                    }).ToArray()
                }
            ).ToArray());
        }

        #endregion
    }
}
