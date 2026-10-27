using BloFin.Net.Enums;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.SpotApi
{
    internal partial class BloFinRestClientSpotSharedApi
    {
        #region Transfer

        async Task<IExchangeCallResult<SharedId>> ITransfer.TransferAsync(TransferRequest request, CancellationToken ct)
            => await TransferAsync(request, ct).ConfigureAwait(false);

        public TransferOptions TransferOptions { get; } = new TransferOptions(_exchangeName, [
            SharedAccountType.PerpetualLinearFutures,
            SharedAccountType.PerpetualInverseFutures,
            SharedAccountType.DeliveryLinearFutures,
            SharedAccountType.DeliveryInverseFutures,
            SharedAccountType.Spot,
            SharedAccountType.Funding
            ])
        {
            ParameterRuleOverrides = [
                RequestParameterRuleOverride<TransferRequest>.NotSupported(x => x.FromSymbol),
                RequestParameterRuleOverride<TransferRequest>.NotSupported(x => x.ToSymbol),
                ]
        };

        public async Task<HttpResult<SharedId>> TransferAsync(TransferRequest request, CancellationToken ct)
        {
            var validationError = TransferOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var fromType = GetTransferType(request.FromAccountType);
            var toType = GetTransferType(request.ToAccountType);
            if (fromType == null || toType == null)
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid("To/From AccountType", "invalid to/from account combination"));

            // Get data
            var transfer = await _api.Account.TransferAsync(
                request.Asset,
                fromType.Value,
                toType.Value,
                request.Quantity,
                ct: ct).ConfigureAwait(false);
            if (!transfer.Success)
                return HttpResult.Fail<SharedId>(transfer);

            return HttpResult.Ok(transfer, new SharedId(transfer.Data.TransferId));
        }

        private AccountType? GetTransferType(SharedAccountType fromAccountType)
            => fromAccountType switch
            {
                SharedAccountType.PerpetualLinearFutures => AccountType.Futures,
                SharedAccountType.PerpetualInverseFutures => AccountType.InverseContract,
                SharedAccountType.DeliveryLinearFutures => AccountType.Futures,
                SharedAccountType.DeliveryInverseFutures => AccountType.InverseContract,
                SharedAccountType.Spot => AccountType.Spot,
                SharedAccountType.Funding => AccountType.Funding,
                _ => null
            };

        #endregion

        #region Get Transfer History

        async Task<IExchangeCallResult<SharedTransfer[]>> IGetTransferHistory.GetTransferHistoryAsync(GetTransferHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetTransferHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public GetTransferHistoryOptions GetTransferHistoryOptions { get; } = new GetTransferHistoryOptions(_exchangeName, false, true, true, 100);
        public async Task<HttpResult<SharedTransfer[]>> GetTransferHistoryAsync(GetTransferHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetTransferHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTransfer[]>(Exchange, validationError);

            // Determine page token
            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Account.GetTransferHistoryAsync(
                startTime: pageParams.StartTime ?? pageParams.EndTime?.AddDays(-3),
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTransfer[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                         () => Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.Timestamp)),
                         result.Data.Length,
                         result.Data.Select(x => x.Timestamp),
                         request.StartTime,
                         request.EndTime ?? DateTime.UtcNow,
                         pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                .Select(x =>
                    new SharedTransfer(
                        x.Asset,
                        x.Quantity,
                        ParseAccountType(x.FromAccount),
                        ParseAccountType(x.ToAccount),
                        x.Timestamp)
                    {
                    })
                .ToArray(), nextPageRequest);
        }

        private SharedAccountType ParseAccountType(AccountType? accountType)
        {
            if (accountType == AccountType.Spot)
                return SharedAccountType.Spot;
            if (accountType == AccountType.Funding)
                return SharedAccountType.Funding;
            if (accountType == AccountType.Futures)
                return SharedAccountType.PerpetualLinearFutures;
            if (accountType == AccountType.InverseContract)
                return SharedAccountType.PerpetualInverseFutures;

            return SharedAccountType.Other;
        }

        #endregion
    }
}
