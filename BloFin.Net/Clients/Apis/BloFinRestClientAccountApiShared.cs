using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.AccountApi;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.Apis
{
    internal partial class BloFinRestClientAccountApi : IBloFinRestClientAccountApiShared
    {
        public string Exchange => "BloFin";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Withdrawal client

        GetWithdrawalsOptions IWithdrawalRestClient.GetWithdrawalsOptions { get; } = new GetWithdrawalsOptions(SharedPaginationSupport.Descending, true, 100);
        async Task<ExchangeWebResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IWithdrawalRestClient)this).GetWithdrawalsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedWithdrawal[]>(Exchange, validationError);

            // Determine page token
            DateTime? endTime = null;
            if (pageToken is DateTimeToken dateToken)
                endTime = dateToken.LastTime;

            // Get data
            var withdrawals = await GetWithdrawalHistoryAsync(
                request.Asset,
                startTime: request.StartTime,
                endTime: endTime ?? request.EndTime,
                limit: request.Limit ?? 100,                 
                ct: ct).ConfigureAwait(false);
            if (!withdrawals)
                return withdrawals.AsExchangeResult<SharedWithdrawal[]>(Exchange, null, default);

            // Determine next token
            DateTimeToken? nextToken = null;
            if (withdrawals.Data.Count() == (request.Limit ?? 100))
                nextToken = new DateTimeToken(withdrawals.Data.Min(x => x.Timestamp));

            return withdrawals.AsExchangeResult(Exchange, TradingMode.Spot, withdrawals.Data.Select(x => 
            new SharedWithdrawal(
                x.Asset,
                x.Address,
                x.Quantity, 
                x.Status == WithdrawalStatus.Success,
                x.Timestamp)
            {
                Network = x.Network,
                Tag = x.Tag,
                TransactionId = x.TransactionId,
                Fee = x.Fee,                
                Id = x.WithdrawId
            }).ToArray(), nextToken);
        }

        #endregion

        #region Deposit client

        EndpointOptions<GetDepositAddressesRequest> IDepositRestClient.GetDepositAddressesOptions { get; } = new EndpointOptions<GetDepositAddressesRequest>(true)
        {
            Supported = false
        };
        Task<ExchangeWebResult<SharedDepositAddress[]>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            return Task.FromResult(new ExchangeWebResult<SharedDepositAddress[]>(Exchange, new InvalidOperationError($"Method not available for {Exchange}")));
        }

        GetDepositsOptions IDepositRestClient.GetDepositsOptions { get; } = new GetDepositsOptions(SharedPaginationSupport.Descending, true, 100);
        async Task<ExchangeWebResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IDepositRestClient)this).GetDepositsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedDeposit[]>(Exchange, validationError);

            // Determine page token
            DateTime? endTime = null;
            if (pageToken is DateTimeToken dateToken)
                endTime = dateToken.LastTime;

            // Get data
            var deposits = await GetDepositHistoryAsync(
                request.Asset,
                startTime: request.StartTime,
                endTime: endTime ?? request.EndTime,
                limit: request.Limit ?? 100,
                ct: ct).ConfigureAwait(false);
            if (!deposits)
                return deposits.AsExchangeResult<SharedDeposit[]>(Exchange, null, default);

            // Determine next token
            DateTimeToken? nextToken = null;
            if (deposits.Data.Count() == (request.Limit ?? 100))
                nextToken = new DateTimeToken(deposits.Data.Min(x => x.Timestamp));

            return deposits.AsExchangeResult(Exchange, TradingMode.Spot, deposits.Data.Select(x => 
                new SharedDeposit(
                    x.Asset, 
                    x.Quantity,
                    x.Status == DepositStatus.Done, 
                    x.Timestamp)
            {
                Confirmations = x.Confirmations,
                Network = x.Network,
                TransactionId = x.TransactionId,
                Id = x.DepositId
            }).ToArray(), nextToken);
        }

        #endregion
    }
}
