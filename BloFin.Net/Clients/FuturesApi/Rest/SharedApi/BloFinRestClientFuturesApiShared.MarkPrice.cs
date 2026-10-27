using BloFin.Net.Enums;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.FuturesApi
{
    internal partial class BloFinRestClientFuturesSharedApi
    {
        #region Get Mark Price

        async Task<IExchangeCallResult<SharedMarkPrice>> IGetMarkPrice.GetMarkPriceAsync(GetMarkPriceRequest request, CancellationToken ct)
            => await ((IGetMarkPrice)this).GetMarkPriceAsync(request, ct).ConfigureAwait(false);

        public GetMarkPriceOptions GetMarkPriceOptions { get; } = new GetMarkPriceOptions(_exchangeName, false);
        public async Task<HttpResult<SharedMarkPrice>> GetMarkPriceAsync(GetMarkPriceRequest request, CancellationToken ct)
        {
            var validationError = GetMarkPriceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedMarkPrice>(Exchange, validationError);

            var result = await _api.ExchangeData.GetIndexMarkPriceAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedMarkPrice>(result);

            var ticker = result.Data.SingleOrDefault();
            if (ticker == null)
                return HttpResult.Fail<SharedMarkPrice>(result, new ServerError(ErrorType.UnknownSymbol, "No ticker found for symbol"));

            return HttpResult.Ok(result, new SharedMarkPrice(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, ticker.Symbol),
                    ticker.Symbol,
                    ticker.MarkPrice));
        }


        #endregion

        #region Get All Mark Prices

        async Task<IExchangeCallResult<SharedMarkPrice[]>> IGetAllMarkPrices.GetAllMarkPricesAsync(GetAllMarkPricesRequest request, CancellationToken ct)
            => await ((IGetAllMarkPrices)this).GetAllMarkPricesAsync(request, ct).ConfigureAwait(false);

        public GetAllMarkPricesOptions GetAllMarkPricesOptions { get; } = new GetAllMarkPricesOptions(_exchangeName, false);
        public async Task<HttpResult<SharedMarkPrice[]>> GetAllMarkPricesAsync(GetAllMarkPricesRequest request, CancellationToken ct)
        {
            var validationError = GetAllMarkPricesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedMarkPrice[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetIndexMarkPriceAsync(
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedMarkPrice[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x =>
                new SharedMarkPrice(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.MarkPrice)
            ).ToArray());
        }

        #endregion
    }
}
