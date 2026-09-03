using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BloFin.Net.Clients.FuturesApi
{
    internal partial class BloFinRestClientFuturesSharedApi
    {
        #region Position History client

        public GetPositionHistoryOptions GetPositionHistoryOptions { get; } = new GetPositionHistoryOptions(_exchangeName, false, false, true, 100);
        public async Task<HttpResult<SharedPositionHistory[]>> GetPositionHistoryAsync(GetPositionHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetPositionHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPositionHistory[]>(Exchange, validationError);

            // Pagination doesn't work correctly. When setting beforeId it still returns data after it

            var direction = DataDirection.Descending;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var limit = request.Limit ?? 100;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Trading.GetPositionHistoryAsync(
                symbol: request.Symbol!.GetSymbol(FormatSymbol),
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                beforeId: pageParams.FromId == null ? null : long.Parse(pageParams.FromId),
                limit: pageParams.Limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedPositionHistory[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromId(result.Data.Min(x => x.HistoryId)),
                    result.Data.Length,
                    result.Data.Select(x => x.CreateTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.CreateTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedPositionHistory(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                            x.Symbol,
                            x.PositionSide == PositionSide.Net ? (x.ClosePositions >= 0 ? SharedPositionSide.Long : SharedPositionSide.Short) : x.PositionSide == PositionSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long,
                            x.OpenAveragePrice,
                            x.CloseAveragePrice,
                            new SharedOrderQuantity(contractQuantity: x.ClosePositions),
                            x.RealizedPnl,
                            x.CreateTime)
                        {
                            Leverage = x.Leverage,
                            PositionId = x.PositionId.ToString(),
                        })
                    .ToArray(), nextPageRequest);
        }
        #endregion
    }
}
