using BloFin.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.FuturesApi
{
    internal partial class BloFinRestClientFuturesSharedApi
    {
        public SharedLeverageSettingMode LeverageSettingType => SharedLeverageSettingMode.PerSymbol;

        #region Get Leverage

        async Task<IExchangeCallResult<SharedLeverage>> IGetLeverage.GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
            => await GetLeverageAsync(request, ct).ConfigureAwait(false);

        public GetLeverageOptions GetLeverageOptions { get; } = new GetLeverageOptions(_exchangeName, true)
        {
            ParameterRuleOverrides =
            [
                RequestParameterRuleOverride<GetLeverageRequest>.Required(x => x.MarginMode)
            ]
        };
        public async Task<HttpResult<SharedLeverage>> GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
        {
            var validationError = GetLeverageOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverage>(Exchange, validationError);

            var result = await _api.Account.GetLeverageAsync(
                symbol: request.Symbol!.GetSymbol(FormatSymbol),
                marginMode: request.MarginMode == SharedMarginMode.Isolated ? MarginMode.Isolated : MarginMode.Cross, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverage>(result);

            return HttpResult.Ok(result, new SharedLeverage(result.Data.First().Leverage)
            {
                Side = request.PositionSide
            });
        }

        #endregion

        #region Set Leverage

        async Task<IExchangeCallResult<SharedLeverage>> ISetLeverage.SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
            => await SetLeverageAsync(request, ct).ConfigureAwait(false);

        public SetLeverageOptions SetLeverageOptions { get; } = new SetLeverageOptions(_exchangeName)
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<SetLeverageRequest>.Required(x => x.MarginMode)
            ]   
        };
        public async Task<HttpResult<SharedLeverage>> SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
        {
            var validationError = SetLeverageOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverage>(Exchange, validationError);

            var result = await _api.Account.SetLeverageAsync(
                symbol: request.Symbol!.GetSymbol(FormatSymbol),
                leverage: (int)request.Leverage,
                marginMode: request.MarginMode == SharedMarginMode.Isolated ? MarginMode.Isolated : MarginMode.Cross,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverage>(result);

            return HttpResult.Ok(result, new SharedLeverage(result.Data.Leverage));
        }

        #endregion

        #region Get Leverage Tiers

        async Task<IExchangeCallResult<SharedLeverageTier[]>> IGetLeverageTiers.GetLeverageTiersAsync(GetLeverageTiersRequest request, CancellationToken ct)
            => await GetLeverageTiersAsync(request, ct).ConfigureAwait(false);

        public GetLeverageTiersOptions GetLeverageTiersOptions { get; } = new GetLeverageTiersOptions(_exchangeName, true)
        {
            ExchangeParameterRules = [
                ExchangeParameterRule.Required<SharedMarginMode>("MarginMode", "Margin mode", SharedMarginMode.Cross)
                ]
        };
        public async Task<HttpResult<SharedLeverageTier[]>> GetLeverageTiersAsync(GetLeverageTiersRequest request, CancellationToken ct)
        {
            var validationError = GetLeverageTiersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverageTier[]>(Exchange, validationError);

            var marginMode = request.GetParamValue<SharedMarginMode>("MarginMode");
            var result = await _api.ExchangeData.GetPositionTiersAsync(
                symbol: request.Symbol!.GetSymbol(FormatSymbol),
                marginMode == SharedMarginMode.Cross ? MarginMode.Cross : MarginMode.Isolated,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverageTier[]>(result);

            int index = 0;
            return HttpResult.Ok(result, result.Data.Select(x => {
                return new SharedLeverageTier(request.Symbol, x.Symbol, index++, null, x.MinSize ?? 0, x.MaxSize, x.MaintenanceMarginRate, x.MaxLeverage);
                }
            ).ToArray());
        }

        #endregion
    }
}
