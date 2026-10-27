using BloFin.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.FuturesApi
{
    internal partial class BloFinRestClientFuturesSharedApi
    {
        public SharedPositionModeSelection PositionModeSettingType => SharedPositionModeSelection.PerAccount;

        #region Get Position Mode

        async Task<IExchangeCallResult<SharedPositionModeResult>> IGetPositionMode.GetPositionModeAsync(GetPositionModeRequest request, CancellationToken ct)
            => await GetPositionModeAsync(request, ct).ConfigureAwait(false);

        public GetPositionModeOptions GetPositionModeOptions { get; } = new GetPositionModeOptions(_exchangeName);
        public async Task<HttpResult<SharedPositionModeResult>> GetPositionModeAsync(GetPositionModeRequest request, CancellationToken ct)
        {
            var validationError = GetPositionModeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPositionModeResult>(Exchange, validationError);

            var result = await _api.Account.GetPositionModeAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedPositionModeResult>(result);

            return HttpResult.Ok(result, new SharedPositionModeResult(result.Data.PositionMode == PositionMode.HedgeMode ? SharedPositionMode.HedgeMode : SharedPositionMode.OneWay));
        }

        #endregion

        #region Set Position Mode

        async Task<IExchangeCallResult<SharedPositionModeResult>> ISetPositionMode.SetPositionModeAsync(SetPositionModeRequest request, CancellationToken ct)
            => await SetPositionModeAsync(request, ct).ConfigureAwait(false);

        public SetPositionModeOptions SetPositionModeOptions { get; } = new SetPositionModeOptions(_exchangeName);
        public async Task<HttpResult<SharedPositionModeResult>> SetPositionModeAsync(SetPositionModeRequest request, CancellationToken ct)
        {
            var validationError = SetPositionModeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPositionModeResult>(Exchange, validationError);

            var result = await _api.Account.SetPositionModeAsync(request.PositionMode == SharedPositionMode.HedgeMode ? PositionMode.HedgeMode : PositionMode.OneWayMode, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedPositionModeResult>(result);

            return HttpResult.Ok(result, new SharedPositionModeResult(request.PositionMode));
        }

        #endregion
    }
}
