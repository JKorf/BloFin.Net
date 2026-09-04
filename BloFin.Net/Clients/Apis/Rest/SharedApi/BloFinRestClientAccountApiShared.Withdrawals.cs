using BloFin.Net.Clients.FuturesApi;
using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.AccountApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.Apis
{
    internal partial class BloFinRestClientAccountSharedApi
    {

        #region Get Withdrawal History

        async Task<ICallResult<SharedWithdrawal[]>> IGetWithdrawalHistory.GetWithdrawalHistoryAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetWithdrawalHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        Task<HttpResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, PageRequest? nextPageToken, CancellationToken ct)
            => GetWithdrawalHistoryAsync(request, nextPageToken, ct);
        GetWithdrawalHistoryOptions IWithdrawalRestClient.GetWithdrawalsOptions => GetWithdrawalHistoryOptions;

        public GetWithdrawalHistoryOptions GetWithdrawalHistoryOptions { get; } = new GetWithdrawalHistoryOptions(_exchangeName, false, true, true, 100);
        public async Task<HttpResult<SharedWithdrawal[]>> GetWithdrawalHistoryAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetWithdrawalHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedWithdrawal[]>(Exchange, validationError);

            var direction = DataDirection.Descending;
            var limit = request.Limit ?? 100;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.GetWithdrawalHistoryAsync(
                request.Asset,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,                 
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedWithdrawal[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.Timestamp)),
                result.Data.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                       .Select(x => 
                            new SharedWithdrawal(
                                x.Asset,
                                x.Address,
                                x.Quantity, 
                                x.Status == WithdrawalStatus.Success,
                                x.Timestamp,
                                GetWithdrawalStatus(x))
                            {
                                Network = x.Network,
                                Tag = x.Tag,
                                TransactionId = x.TransactionId,
                                Fee = x.Fee,                
                                Id = x.WithdrawId
                            })
                       .ToArray(), nextPageRequest);
        }

        #endregion

        private SharedTransferStatus GetWithdrawalStatus(BloFinWithdrawal x)
        {
            if (x.Status == WithdrawalStatus.Canceled || x.Status == WithdrawalStatus.Failed)
                return SharedTransferStatus.Failed;

            if (x.Status == WithdrawalStatus.Success)
                return SharedTransferStatus.Completed;

            if (x.Status == WithdrawalStatus.WaitingReview || x.Status == WithdrawalStatus.Kyt || x.Status == WithdrawalStatus.Processing)
                return SharedTransferStatus.InProgress;

            return SharedTransferStatus.Unknown;
        }

    }
}
