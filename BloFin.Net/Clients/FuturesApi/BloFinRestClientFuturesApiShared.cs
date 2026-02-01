using BloFin.Net.Enums;
using BloFin.Net.Interfaces.Clients.FuturesApi;
using BloFin.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BloFin.Net.Clients.FuturesApi
{
    internal partial class BloFinRestClientFuturesApi : IBloFinRestClientFuturesApiShared
    {
        private const string _topicId = "BloFinFutures";
        public string Exchange => "BloFin";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.PerpetualLinear, TradingMode.PerpetualInverse };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Book Ticker client

        EndpointOptions<GetBookTickerRequest> IBookTickerRestClient.GetBookTickerOptions { get; } = new EndpointOptions<GetBookTickerRequest>(false);
        async Task<ExchangeWebResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = ((IBookTickerRestClient)this).GetBookTickerOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedBookTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetTickersAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!resultTicker)
                return resultTicker.AsExchangeResult<SharedBookTicker>(Exchange, null, default);

            var ticker = resultTicker.Data.SingleOrDefault();
            if (ticker == null)
                return resultTicker.AsExchangeError<SharedBookTicker>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Ticker not found")));

            return resultTicker.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, ticker.Symbol),
                ticker.Symbol,
                ticker.BestAskPrice,
                ticker.BestAskQuantity,
                ticker.BestBidPrice,
                ticker.BestBidQuantity));
        }

        #endregion

        #region Klines client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(SharedPaginationSupport.Descending, true, 1440, false);

        async Task<ExchangeWebResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeWebResult<SharedKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported"));

            var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedKline[]>(Exchange, validationError);

            var limit = request.Limit ?? 1440;
            DateTime endTime = request.EndTime ?? DateTime.UtcNow;
            DateTime? startTime = request.StartTime;
            if (pageToken is DateTimeToken dateTimeToken)
                endTime = dateTimeToken.LastTime;

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                interval,
                startTime,
                endTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedKline[]>(Exchange, null, default);

            // Get next token
            DateTimeToken? nextToken = null;
            if (result.Data.Count() == limit)
            {
                var minOpenTime = result.Data.Min(x => x.OpenTime);
                if (request.StartTime == null || minOpenTime > request.StartTime.Value)
                    nextToken = new DateTimeToken(minOpenTime.AddSeconds(-(int)(interval - 1)));
            }

            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, result.Data.Select(x => 
                new SharedKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume)).ToArray(), nextToken);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(1, 100, false);
        async Task<ExchangeWebResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = ((IOrderBookRestClient)this).GetOrderBookOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                depth: request.Limit ?? 20,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedOrderBook>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedOrderBook(result.Data.Asks, result.Data.Bids));
        }

        #endregion

        #region Recent Trade client

        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(100, false);
        async Task<ExchangeWebResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IRecentTradeRestClient)this).GetRecentTradesOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetRecentTradesAsync(
                symbol,
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedTrade[]>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, result.Data.Select(x => 
            new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
            {
                Side = x.Side == Enums.OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            }).ToArray());
        }

        #endregion

        #region Funding Rate client
        GetFundingRateHistoryOptions IFundingRateRestClient.GetFundingRateHistoryOptions { get; } = new GetFundingRateHistoryOptions(SharedPaginationSupport.Descending, true, 100, false);

        async Task<ExchangeWebResult<SharedFundingRate[]>> IFundingRateRestClient.GetFundingRateHistoryAsync(GetFundingRateHistoryRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IFundingRateRestClient)this).GetFundingRateHistoryOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFundingRate[]>(Exchange, validationError);

            DateTime? endTime = null;
            if (pageToken is DateTimeToken token)
                endTime = token.LastTime;

            // Get data
            var result = await ExchangeData.GetFundingRateHistoryAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                startTime: request.StartTime,
                endTime: endTime ?? request.EndTime,
                limit: request.Limit ?? 100,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFundingRate[]>(Exchange, null, default);

            DateTimeToken? nextToken = null;
            if (result.Data.Count() == (request.Limit ?? 100))
                nextToken = new DateTimeToken(result.Data.Min(x => x.FundingTime).AddSeconds(-1));

            // Return
            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, result.Data.Select(x => new SharedFundingRate(x.FundingRate, x.FundingTime)).ToArray(), nextToken);
        }
        #endregion

        #region Futures Symbol client

        EndpointOptions<GetSymbolsRequest> IFuturesSymbolRestClient.GetFuturesSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest>(false);
        async Task<ExchangeWebResult<SharedFuturesSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesSymbolRestClient)this).GetFuturesSymbolsOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesSymbol[]>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFuturesSymbol[]>(Exchange, null, default);

            IEnumerable<BloFinSymbol> data = result.Data;
            if (request.TradingMode != null)
                data = data.Where(x => request.TradingMode == TradingMode.PerpetualLinear || request.TradingMode == TradingMode.DeliveryLinear ? x.ContractType == ContractType.Linear : x.ContractType == ContractType.Inverse);

            var resultData = result.AsExchangeResult(Exchange, request.TradingMode == null ? SupportedTradingModes : new[] { request.TradingMode.Value }, data.Select(s =>
                new SharedFuturesSymbol(s.ContractType == ContractType.Linear  ? TradingMode.PerpetualLinear : TradingMode.PerpetualInverse,
                s.BaseAsset,
                s.QuoteAsset,
                s.Symbol,
                s.Status == SymbolStatus.Live)
                {
                    MinTradeQuantity = s.MinQuantity,
                    MaxTradeQuantity = Math.Min(s.MaxLimitQuantity, s.MaxMarketQuantity),
                    QuantityStep = s.LotSize,
                    PriceStep = s.TickSize,
                    ContractSize = s.ContractSize,
                    MaxLongLeverage = s.MaxLeverage,
                    MaxShortLeverage = s.MaxLeverage
                }).ToArray());

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, resultData.Data);
            return resultData;
        }

        public async Task<ExchangeResult<SharedSymbol[]>> GetFuturesSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<SharedSymbol[]>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<SharedSymbol[]>(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, baseAsset));
        }

        public async Task<ExchangeResult<bool>> SupportsFuturesSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Spot symbols not allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<bool>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<bool>(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, symbol));
        }

        public async Task<ExchangeResult<bool>> SupportsFuturesSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<bool>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<bool>(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, symbolName));
        }
        #endregion

        #region Ticker client

        GetTickerOptions IFuturesTickerRestClient.GetFuturesTickerOptions { get; } = new GetTickerOptions();
        async Task<ExchangeWebResult<SharedFuturesTicker>> IFuturesTickerRestClient.GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTickerRestClient)this).GetFuturesTickerOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesTicker>(Exchange, validationError);

            var resultTicker = ExchangeData.GetTickersAsync(request.Symbol!.GetSymbol(FormatSymbol), ct);
            var resultMarkPrice = ExchangeData.GetIndexMarkPriceAsync(request.Symbol!.GetSymbol(FormatSymbol), ct);
            var resultFunding = ExchangeData.GetFundingRateAsync(request.Symbol!.GetSymbol(FormatSymbol), ct);
            await Task.WhenAll(resultTicker, resultMarkPrice, resultFunding).ConfigureAwait(false);

            if (!resultTicker.Result)
                return resultTicker.Result.AsExchangeResult<SharedFuturesTicker>(Exchange, null, default);
            if (!resultMarkPrice.Result)
                return resultMarkPrice.Result.AsExchangeResult<SharedFuturesTicker>(Exchange, null, default);
            if (!resultFunding.Result)
                return resultFunding.Result.AsExchangeResult<SharedFuturesTicker>(Exchange, null, default);

            var ticker = resultTicker.Result.Data.Single();
            return resultTicker.Result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedFuturesTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, ticker.Symbol), ticker.Symbol, ticker.LastPrice, ticker.High24h, ticker.Low24h, ticker.BaseVolume24h, null)
            {
                MarkPrice = resultMarkPrice.Result.Data.MarkPrice,
                IndexPrice = resultMarkPrice.Result.Data.IndexPrice,
                FundingRate = resultFunding.Result.Data.FundingRate,
                NextFundingTime = resultFunding.Result.Data.FundingTime
            });
        }

        GetTickersOptions IFuturesTickerRestClient.GetFuturesTickersOptions { get; } = new GetTickersOptions();
        async Task<ExchangeWebResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTickerRestClient)this).GetFuturesTickersOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesTicker[]>(Exchange, validationError);

            var resultTickers = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!resultTickers)
                return resultTickers.AsExchangeResult<SharedFuturesTicker[]>(Exchange, null, default);

            IEnumerable<BloFinTicker> data = resultTickers.Data;
            if (request.TradingMode.HasValue)
                data = data.Where(x => request.TradingMode == TradingMode.PerpetualInverse ? x.Symbol!.EndsWith("USD") : !x.Symbol!.EndsWith("USD"));

            return resultTickers.AsExchangeResult(Exchange, request.TradingMode == null ? SupportedTradingModes : new[] { request.TradingMode.Value }, data.Select(x =>
            {
                return new SharedFuturesTicker(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol, x.LastPrice, x.High24h, x.Low24h, x.BaseVolume24h, null);
            }).ToArray());
        }

        #endregion

        #region Index Klines client

        GetKlinesOptions IIndexPriceKlineRestClient.GetIndexPriceKlinesOptions { get; } = new GetKlinesOptions(SharedPaginationSupport.Descending, true, 1440, false);

        async Task<ExchangeWebResult<SharedFuturesKline[]>> IIndexPriceKlineRestClient.GetIndexPriceKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeWebResult<SharedFuturesKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported"));

            var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesKline[]>(Exchange, validationError);

            var limit = request.Limit ?? 1440;
            DateTime endTime = request.EndTime ?? DateTime.UtcNow;
            DateTime? startTime = request.StartTime;
            if (pageToken is DateTimeToken dateTimeToken)
                endTime = dateTimeToken.LastTime;

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetIndexPriceKlinesAsync(
                symbol,
                interval,
                startTime,
                endTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFuturesKline[]>(Exchange, null, default);

            // Get next token
            DateTimeToken? nextToken = null;
            if (result.Data.Count() == limit)
            {
                var minOpenTime = result.Data.Min(x => x.OpenTime);
                if (request.StartTime == null || minOpenTime > request.StartTime.Value)
                    nextToken = new DateTimeToken(minOpenTime.AddSeconds(-(int)(interval - 1)));
            }

            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, result.Data.Select(x => 
                new SharedFuturesKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice)).ToArray(), nextToken);
        }

        #endregion

        #region Index Klines client

        GetKlinesOptions IMarkPriceKlineRestClient.GetMarkPriceKlinesOptions { get; } = new GetKlinesOptions(SharedPaginationSupport.Descending, true, 1440, false);

        async Task<ExchangeWebResult<SharedFuturesKline[]>> IMarkPriceKlineRestClient.GetMarkPriceKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeWebResult<SharedFuturesKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported"));

            var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesKline[]>(Exchange, validationError);

            var limit = request.Limit ?? 1440;
            DateTime endTime = request.EndTime ?? DateTime.UtcNow;
            DateTime? startTime = request.StartTime;
            if (pageToken is DateTimeToken dateTimeToken)
                endTime = dateTimeToken.LastTime;

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetMarkPriceKlinesAsync(
                symbol,
                interval,
                startTime,
                endTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFuturesKline[]>(Exchange, null, default);

            // Get next token
            DateTimeToken? nextToken = null;
            if (result.Data.Count() == limit)
            {
                var minOpenTime = result.Data.Min(x => x.OpenTime);
                if (request.StartTime == null || minOpenTime > request.StartTime.Value)
                    nextToken = new DateTimeToken(minOpenTime.AddSeconds(-(int)(interval - 1)));
            }

            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, result.Data.Select(x => 
                new SharedFuturesKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice)).ToArray(), nextToken);
        }

        #endregion

        #region Balance Client
        GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new GetBalancesOptions(AccountTypeFilter.Futures);

        async Task<ExchangeWebResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = ((IBalanceRestClient)this).GetBalancesOptions.ValidateRequest(Exchange, request, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedBalance[]>(Exchange, validationError);

            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedBalance[]>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, SupportedTradingModes, result.Data.Details.Select(x => new SharedBalance(x.Asset, x.Available, x.Balance)).ToArray());
        }

        #endregion

        #region Position Mode client
        SharedPositionModeSelection IPositionModeRestClient.PositionModeSettingType => SharedPositionModeSelection.PerAccount;

        GetPositionModeOptions IPositionModeRestClient.GetPositionModeOptions { get; } = new GetPositionModeOptions();
        async Task<ExchangeWebResult<SharedPositionModeResult>> IPositionModeRestClient.GetPositionModeAsync(GetPositionModeRequest request, CancellationToken ct)
        {
            var validationError = ((IPositionModeRestClient)this).GetPositionModeOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedPositionModeResult>(Exchange, validationError);

            var result = await Account.GetPositionModeAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedPositionModeResult>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, SupportedTradingModes, new SharedPositionModeResult(result.Data.PositionMode == PositionMode.HedgeMode ? SharedPositionMode.HedgeMode : SharedPositionMode.OneWay));
        }

        SetPositionModeOptions IPositionModeRestClient.SetPositionModeOptions { get; } = new SetPositionModeOptions();
        async Task<ExchangeWebResult<SharedPositionModeResult>> IPositionModeRestClient.SetPositionModeAsync(SetPositionModeRequest request, CancellationToken ct)
        {
            var validationError = ((IPositionModeRestClient)this).SetPositionModeOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedPositionModeResult>(Exchange, validationError);

            var result = await Account.SetPositionModeAsync(request.PositionMode == SharedPositionMode.HedgeMode ? PositionMode.HedgeMode : PositionMode.OneWayMode, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedPositionModeResult>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, SupportedTradingModes, new SharedPositionModeResult(request.PositionMode));
        }
        #endregion

        #region Leverage client
        SharedLeverageSettingMode ILeverageRestClient.LeverageSettingType => SharedLeverageSettingMode.PerSymbol;

        EndpointOptions<GetLeverageRequest> ILeverageRestClient.GetLeverageOptions { get; } = new EndpointOptions<GetLeverageRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(GetLeverageRequest.MarginMode), typeof(SharedMarginMode), "Margin mode", SharedMarginMode.Cross)
            }
        };
        async Task<ExchangeWebResult<SharedLeverage>> ILeverageRestClient.GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
        {
            var validationError = ((ILeverageRestClient)this).GetLeverageOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedLeverage>(Exchange, validationError);

            var result = await Account.GetLeverageAsync(
                symbol: request.Symbol!.GetSymbol(FormatSymbol),
                marginMode: request.MarginMode == SharedMarginMode.Isolated ? MarginMode.Isolated : MarginMode.Cross, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedLeverage>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedLeverage(result.Data.First().Leverage)
            {
                Side = request.PositionSide
            });
        }

        SetLeverageOptions ILeverageRestClient.SetLeverageOptions { get; } = new SetLeverageOptions()
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SetLeverageRequest.MarginMode), typeof(SharedMarginMode), "Margin mode", SharedMarginMode.Cross)
            }
        };
        async Task<ExchangeWebResult<SharedLeverage>> ILeverageRestClient.SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
        {
            var validationError = ((ILeverageRestClient)this).SetLeverageOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedLeverage>(Exchange, validationError);

            var result = await Account.SetLeverageAsync(
                symbol: request.Symbol!.GetSymbol(FormatSymbol),
                leverage: (int)request.Leverage,
                marginMode: request.MarginMode == SharedMarginMode.Isolated ? MarginMode.Isolated : MarginMode.Cross,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedLeverage>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedLeverage(result.Data.Leverage));
        }
        #endregion

        #region Futures Order Client

        SharedFeeDeductionType IFuturesOrderRestClient.FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        SharedFeeAssetType IFuturesOrderRestClient.FuturesFeeAssetType => SharedFeeAssetType.QuoteAsset;

        SharedOrderType[] IFuturesOrderRestClient.FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        SharedTimeInForce[] IFuturesOrderRestClient.FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };
        SharedQuantitySupport IFuturesOrderRestClient.FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts);

        string IFuturesOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(20);

        PlaceFuturesOrderOptions IFuturesOrderRestClient.PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(false)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(PlaceFuturesOrderRequest.MarginMode), typeof(SharedMarginMode), "Margin mode", SharedMarginMode.Cross)
            }
        };
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).PlaceFuturesOrderOptions.ValidateRequest(
                Exchange,
                request,
                request.Symbol!.TradingMode,
                SupportedTradingModes,
                ((IFuturesOrderRestClient)this).FuturesSupportedOrderTypes,
                ((IFuturesOrderRestClient)this).FuturesSupportedTimeInForce,
                ((IFuturesOrderRestClient)this).FuturesSupportedOrderQuantity);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                GetOrderType(request.OrderType, request.TimeInForce),
                marginMode: request.MarginMode == SharedMarginMode.Isolated ? MarginMode.Isolated: MarginMode.Cross,
                quantity: request.Quantity?.QuantityInContracts ?? 0,
                price: request.Price,
                positionSide: request.PositionSide == null ? null : request.PositionSide == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,
                reduceOnly: request.ReduceOnly,
                clientOrderId: request.ClientOrderId,
                takeProfitTriggerPrice: request.TakeProfitPrice,
                stopLossTriggerPrice: request.StopLossPrice,
                ct: ct).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedId(result.Data.OrderId.ToString()));
        }

        EndpointOptions<GetOrderRequest> IFuturesOrderRestClient.GetFuturesOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedFuturesOrder>> IFuturesOrderRestClient.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesOrderOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder>(Exchange, validationError);

            var orders = await Trading.GetOpenOrdersAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                beforeId: (long.Parse(request.OrderId) + 1).ToString(),
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedFuturesOrder>(Exchange, null, default);

            var orderInfo = orders.Data.SingleOrDefault(x => x.OrderId == request.OrderId);
            if (orderInfo == null)
            {
                var closedOrders = await Trading.GetClosedOrdersAsync(
                    request.Symbol.GetSymbol(FormatSymbol),
                    beforeId: (long.Parse(request.OrderId) + 1).ToString(),
                    ct: ct).ConfigureAwait(false);

                if (!closedOrders)
                    return closedOrders.AsExchangeResult<SharedFuturesOrder>(Exchange, null, default);

                orderInfo = closedOrders.Data.SingleOrDefault(x => x.OrderId == request.OrderId);
            }

            if (orderInfo == null)
                return new ExchangeWebResult<SharedFuturesOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return orders.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, orderInfo.Symbol), orderInfo.Symbol,
                orderInfo.OrderId.ToString(),
                ParseOrderType(orderInfo.OrderType),
                orderInfo.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(orderInfo.Status),
                orderInfo.CreateTime)
            {
                ClientOrderId = string.IsNullOrEmpty(orderInfo.ClientOrderId) ? null : orderInfo.ClientOrderId,
                AveragePrice = orderInfo.AveragePrice == 0 ? null : orderInfo.AveragePrice,
                OrderPrice = orderInfo.Price == 0 ? null : orderInfo.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: orderInfo.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: orderInfo.QuantityFilled),
                TimeInForce = ParseTimeInForce(orderInfo.OrderType),
                UpdateTime = orderInfo.UpdateTime,
                PositionSide = orderInfo.PositionSide == PositionSide.Net ? null : orderInfo.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                ReduceOnly = orderInfo.ReduceOnly,
                StopLossPrice = orderInfo.StopLossTriggerPrice,
                TakeProfitPrice = orderInfo.TakeProfitTriggerPrice,
                Fee = orderInfo.Fee,
                Leverage = orderInfo.Leverage                
            });
        }

        EndpointOptions<GetOpenOrdersRequest> IFuturesOrderRestClient.GetOpenFuturesOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest>(true);
        async Task<ExchangeWebResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetOpenFuturesOrdersOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOpenOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedFuturesOrder[]>(Exchange, null, default);

            return orders.AsExchangeResult(Exchange, request.Symbol == null ? SupportedTradingModes : new[] { request.Symbol!.TradingMode }, orders.Data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol,
                x.OrderId.ToString(),
                ParseOrderType(x.OrderType),
                x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = string.IsNullOrEmpty(x.ClientOrderId) ? null : x.ClientOrderId,
                AveragePrice = x.AveragePrice == 0 ? null : x.AveragePrice,
                OrderPrice = x.Price == 0 ? null : x.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                TimeInForce = ParseTimeInForce(x.OrderType),
                UpdateTime = x.UpdateTime,
                PositionSide = x.PositionSide == PositionSide.Net ? null : x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                ReduceOnly = x.ReduceOnly,
                StopLossPrice = x.StopLossTriggerPrice,
                TakeProfitPrice = x.TakeProfitTriggerPrice,
                Fee = x.Fee,
                Leverage = x.Leverage
            }).ToArray());
        }

        PaginatedEndpointOptions<GetClosedOrdersRequest> IFuturesOrderRestClient.GetClosedFuturesOrdersOptions { get; } = new PaginatedEndpointOptions<GetClosedOrdersRequest>(SharedPaginationSupport.Descending, true, 100, true);
        async Task<ExchangeWebResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetClosedFuturesOrdersOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder[]>(Exchange, validationError);

            // Determine page token
            DateTime? fromTimestamp = null;
            if (pageToken is DateTimeToken dateTimeToken)
                fromTimestamp = dateTimeToken.LastTime;

            // Get data
            var orders = await Trading.GetClosedOrdersAsync(request.Symbol!.GetSymbol(FormatSymbol),
                startTime: fromTimestamp ?? request.StartTime,
                endTime: request.EndTime,
                limit: request.Limit ?? 100,
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedFuturesOrder[]>(Exchange, null, default);

            // Get next token
            DateTimeToken? nextToken = null;
            if (orders.Data.Count() == (request.Limit ?? 100))
                nextToken = new DateTimeToken(orders.Data.Max(o => o.CreateTime));

            return orders.AsExchangeResult(Exchange, request.Symbol == null ? SupportedTradingModes : new[] { request.Symbol!.TradingMode }, orders.Data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol,
                x.OrderId.ToString(),
                ParseOrderType(x.OrderType),
                x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = string.IsNullOrEmpty(x.ClientOrderId) ? null : x.ClientOrderId,
                AveragePrice = x.AveragePrice == 0 ? null : x.AveragePrice,
                OrderPrice = x.Price == 0 ? null : x.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                TimeInForce = ParseTimeInForce(x.OrderType),
                UpdateTime = x.UpdateTime,
                PositionSide = x.PositionSide == PositionSide.Net ? null : x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                ReduceOnly = x.ReduceOnly,
                StopLossPrice = x.StopLossTriggerPrice,
                TakeProfitPrice = x.TakeProfitTriggerPrice,
                Fee = x.Fee,
                Leverage = x.Leverage
            }).ToArray(), nextToken);
        }

        EndpointOptions<GetOrderTradesRequest> IFuturesOrderRestClient.GetFuturesOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest>(true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesOrderTradesOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            var orders = await Trading.GetUserTradesAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            return orders.AsExchangeResult(Exchange, request.Symbol!.TradingMode, orders.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol,
                x.OrderId.ToString(),
                x.TradeId.ToString(),
                x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Price = x.Price,
                Quantity = x.Quantity,
                Fee = x.Fee
            }).ToArray());
        }

        PaginatedEndpointOptions<GetUserTradesRequest> IFuturesOrderRestClient.GetFuturesUserTradesOptions { get; } = new PaginatedEndpointOptions<GetUserTradesRequest>(SharedPaginationSupport.Descending, true, 100, true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesUserTradesOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            // Determine page token
            string? fromId = null;
            if (pageToken is FromIdToken fromIdToken)
                fromId = fromIdToken.FromToken;

            // Get data
            var orders = await Trading.GetUserTradesAsync(request.Symbol!.GetSymbol(FormatSymbol),
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: request.Limit ?? 100,
                beforeId: fromId,
                ct: ct
                ).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            // Get next token
            FromIdToken? nextToken = null;
            if (orders.Data.Count() == (request.Limit ?? 100))
                nextToken = new FromIdToken(orders.Data.Max(o => o.TradeId)!);

            return orders.AsExchangeResult(Exchange, request.Symbol!.TradingMode, orders.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol,
                x.OrderId.ToString(),
                x.TradeId.ToString(),
                x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Price = x.Price,
                Quantity = x.Quantity,
                Fee = x.Fee
            }).ToArray(), nextToken);
        }

        EndpointOptions<CancelOrderRequest> IFuturesOrderRestClient.CancelFuturesOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).CancelFuturesOrderOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedId(order.Data.OrderId.ToString()));
        }

        EndpointOptions<GetPositionsRequest> IFuturesOrderRestClient.GetPositionsOptions { get; } = new EndpointOptions<GetPositionsRequest>(true);
        async Task<ExchangeWebResult<SharedPosition[]>> IFuturesOrderRestClient.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetPositionsOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedPosition[]>(Exchange, validationError);

            var result = await Trading.GetPositionsAsync(symbol: request.Symbol?.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedPosition[]>(Exchange, null, default);

            IEnumerable<BloFinPosition> data = result.Data;
            if (request.TradingMode.HasValue)
                data = data.Where(x => request.TradingMode == TradingMode.PerpetualInverse ? x.Symbol!.EndsWith("USD") : !x.Symbol!.EndsWith("USD"));

            var resultTypes = request.Symbol == null && request.TradingMode == null ? SupportedTradingModes : request.Symbol != null ? new[] { request.Symbol!.TradingMode } : new[] { request.TradingMode!.Value };
            return result.AsExchangeResult(Exchange, resultTypes, data.Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol, Math.Abs(x.PositionSize), x.UpdateTime)
            {
                UnrealizedPnl = x.UnrealizedPnl,
                LiquidationPrice = x.LiquidationPrice == 0 ? null : x.LiquidationPrice,
                Leverage = x.Leverage,
                AverageOpenPrice = x.AveragePrice,
                UpdateTime = x.UpdateTime,
                PositionSide = x.PositionSide == PositionSide.Net ? (x.PositionSize >= 0 ? SharedPositionSide.Long : SharedPositionSide.Short) : x.PositionSide == PositionSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long
            }).ToArray());
        }

        EndpointOptions<ClosePositionRequest> IFuturesOrderRestClient.ClosePositionOptions { get; } = new EndpointOptions<ClosePositionRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(ClosePositionRequest.MarginMode), typeof(SharedMarginMode), "The margin mode", SharedMarginMode.Cross),
            }
        };
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).ClosePositionOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var positionMode = await Account.GetPositionModeAsync().ConfigureAwait(false);
            if (!positionMode)
                return positionMode.AsExchangeResult<SharedId>(Exchange, null, default);

            var result = await Trading.ClosePositionAsync(
                symbol,
                request.MarginMode == SharedMarginMode.Isolated ? MarginMode.Isolated : MarginMode.Cross,
                request.PositionSide == null ? null : request.PositionSide == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,                
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedId(null!));
        }

        private OrderType GetOrderType(SharedOrderType orderType, SharedTimeInForce? timeInForce)
        {
            if (orderType == SharedOrderType.Market) return OrderType.Market;
            if (orderType == SharedOrderType.LimitMaker) return OrderType.PostOnly;
            if (timeInForce == SharedTimeInForce.FillOrKill) return OrderType.FillOrKill;
            if (timeInForce == SharedTimeInForce.ImmediateOrCancel) return OrderType.ImmediateOrCancel;
            return OrderType.Limit;
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.Open || status == OrderStatus.PartiallyFilled) return SharedOrderStatus.Open;
            if (status == OrderStatus.Canceled || status == OrderStatus.PartiallyCanceled) return SharedOrderStatus.Canceled;
            return SharedOrderStatus.Filled;
        }

        private SharedOrderType ParseOrderType(OrderType type)
        {
            if (type == OrderType.Market || type == OrderType.ImmediateOrCancel) return SharedOrderType.Market;
            if (type == OrderType.Limit) return SharedOrderType.Limit;
            if (type == OrderType.PostOnly) return SharedOrderType.LimitMaker;

            return SharedOrderType.Other;
        }

        private SharedTimeInForce? ParseTimeInForce(OrderType type)
        {
            if (type == OrderType.Market) return SharedTimeInForce.ImmediateOrCancel;
            if (type == OrderType.ImmediateOrCancel) return SharedTimeInForce.ImmediateOrCancel;
            if (type == OrderType.FillOrKill) return SharedTimeInForce.FillOrKill;
            if (type == OrderType.Limit) return SharedTimeInForce.GoodTillCanceled;

            return null;
        }

        #endregion

        #region Futures Client Id Order Client

        EndpointOptions<GetOrderRequest> IFuturesOrderClientIdRestClient.GetFuturesOrderByClientOrderIdOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedFuturesOrder>> IFuturesOrderClientIdRestClient.GetFuturesOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesOrderOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder>(Exchange, validationError);

            var orders = await Trading.GetOpenOrdersAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                limit: 100,
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedFuturesOrder>(Exchange, null, default);

            var orderInfo = orders.Data.SingleOrDefault(x => x.ClientOrderId == request.OrderId);
            if (orderInfo == null)
            {
                var closedOrders = await Trading.GetClosedOrdersAsync(
                    request.Symbol.GetSymbol(FormatSymbol),
                    limit: 100,
                    ct: ct).ConfigureAwait(false);

                if (!closedOrders)
                    return closedOrders.AsExchangeResult<SharedFuturesOrder>(Exchange, null, default);

                orderInfo = closedOrders.Data.SingleOrDefault(x => x.ClientOrderId == request.OrderId);
            }

            if (orderInfo == null)
                return new ExchangeWebResult<SharedFuturesOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return orders.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, orderInfo.Symbol), orderInfo.Symbol,
                orderInfo.OrderId.ToString(),
                ParseOrderType(orderInfo.OrderType),
                orderInfo.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(orderInfo.Status),
                orderInfo.CreateTime)
            {
                ClientOrderId = string.IsNullOrEmpty(orderInfo.ClientOrderId) ? null : orderInfo.ClientOrderId,
                AveragePrice = orderInfo.AveragePrice == 0 ? null : orderInfo.AveragePrice,
                OrderPrice = orderInfo.Price == 0 ? null : orderInfo.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: orderInfo.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: orderInfo.QuantityFilled),
                TimeInForce = ParseTimeInForce(orderInfo.OrderType),
                UpdateTime = orderInfo.UpdateTime,
                PositionSide = orderInfo.PositionSide == PositionSide.Net ? null : orderInfo.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                ReduceOnly = orderInfo.ReduceOnly,
                StopLossPrice = orderInfo.StopLossTriggerPrice,
                TakeProfitPrice = orderInfo.TakeProfitTriggerPrice,
                Fee = orderInfo.Fee,
                Leverage = orderInfo.Leverage
            });
        }

        EndpointOptions<CancelOrderRequest> IFuturesOrderClientIdRestClient.CancelFuturesOrderByClientOrderIdOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderClientIdRestClient.CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).CancelFuturesOrderOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol), clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedId(order.Data.OrderId.ToString()));
        }
        #endregion

        #region Tp/SL Client
        EndpointOptions<SetTpSlRequest> IFuturesTpSlRestClient.SetFuturesTpSlOptions { get; } = new EndpointOptions<SetTpSlRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SetTpSlRequest.PositionMode), typeof(SharedPositionMode), "Position mode the account is in", SharedPositionMode.OneWay),
                new ParameterDescription(nameof(PlaceFuturesTriggerOrderRequest.MarginMode), typeof(SharedMarginMode), "Margin mode", SharedMarginMode.Cross)
            },
        };

        async Task<ExchangeWebResult<SharedId>> IFuturesTpSlRestClient.SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTpSlRestClient)this).SetFuturesTpSlOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceTpSlOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.PositionSide == SharedPositionSide.Long ? OrderSide.Sell : OrderSide.Buy,
                request.MarginMode == SharedMarginMode.Isolated ? MarginMode.Isolated : MarginMode.Cross,
                positionSide: request.PositionMode == SharedPositionMode.OneWay ? null : request.PositionSide == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,
                reduceOnly: true,
                quantity: request.Quantity,
                takeProfitTriggerPrice: request.TpSlSide == SharedTpSlSide.TakeProfit ? request.TriggerPrice: null,
                stopLossTriggerPrice: request.TpSlSide == SharedTpSlSide.StopLoss ? request.TriggerPrice: null,
                ct: ct).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            // Return
            return result.AsExchangeResult(Exchange, SupportedTradingModes, new SharedId(result.Data.TpslOrderId.ToString()));
        }

        EndpointOptions<CancelTpSlRequest> IFuturesTpSlRestClient.CancelFuturesTpSlOptions { get; } = new EndpointOptions<CancelTpSlRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(CancelTpSlRequest.OrderId), typeof(string), "Id of the tp/sl order", "123123")
            }
        };

        async Task<ExchangeWebResult<bool>> IFuturesTpSlRestClient.CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTpSlRestClient)this).CancelFuturesTpSlOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<bool>(Exchange, validationError);

            var result = await Trading.CancelTpSlOrdersAsync([
                    new BloFinCancelTpSlRequest
                    {
                        OrderId = request.OrderId,
                        Symbol = request.Symbol.GetSymbol(FormatSymbol)
                    }
                ],
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<bool>(Exchange, null, default);

            // Return
            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, true);
        }

        #endregion

        #region Trigger Order Client
        PlaceFuturesTriggerOrderOptions IFuturesTriggerOrderRestClient.PlaceFuturesTriggerOrderOptions { get; } = new PlaceFuturesTriggerOrderOptions(false)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(PlaceFuturesTriggerOrderRequest.PositionMode), typeof(SharedPositionMode), "PositionMode the account is in", SharedPositionMode.OneWay),
                new ParameterDescription(nameof(PlaceFuturesTriggerOrderRequest.MarginMode), typeof(SharedMarginMode), "Margin mode", SharedMarginMode.Cross)
            }
        };
        async Task<ExchangeWebResult<SharedId>> IFuturesTriggerOrderRestClient.PlaceFuturesTriggerOrderAsync(PlaceFuturesTriggerOrderRequest request, CancellationToken ct)
        {
            var side = GetTriggerOrderParameters(request.OrderDirection, request.PositionSide);
            var validationError = ((IFuturesTriggerOrderRestClient)this).PlaceFuturesTriggerOrderOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes, side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell, ((IFuturesOrderRestClient)this).FuturesSupportedOrderQuantity);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceTriggerOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                side,
                request.MarginMode == SharedMarginMode.Isolated ? MarginMode.Isolated : MarginMode.Cross,
                request.TriggerPrice,
                request.TriggerPriceType == null ? null : request.TriggerPriceType == SharedTriggerPriceType.LastPrice ? TriggerPriceType.LastPrice : request.TriggerPriceType == SharedTriggerPriceType.MarkPrice ? TriggerPriceType.MarkPrice : TriggerPriceType.IndexPrice,
                request.PositionMode == SharedPositionMode.OneWay ? null : request.PositionSide == SharedPositionSide.Long ? PositionSide.Long : PositionSide.Short,
                request.Quantity.QuantityInContracts,
                orderPrice: request.OrderPrice,
                reduceOnly: request.OrderDirection == SharedTriggerOrderDirection.Exit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            // Return
            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedId(result.Data.AlgoOrderId.ToString()));
        }

        EndpointOptions<GetOrderRequest> IFuturesTriggerOrderRestClient.GetFuturesTriggerOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedFuturesTriggerOrder>> IFuturesTriggerOrderRestClient.GetFuturesTriggerOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTriggerOrderRestClient)this).GetFuturesTriggerOrderOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesTriggerOrder>(Exchange, validationError);

            var order = await Trading.GetOpenTriggerOrdersAsync(orderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedFuturesTriggerOrder>(Exchange, null, default);

            if (!order.Data.Any())
            {

                order = await Trading.GetClosedTriggerOrdersAsync(orderId: request.OrderId, ct: ct).ConfigureAwait(false);
                if (!order)
                    return order.AsExchangeResult<SharedFuturesTriggerOrder>(Exchange, null, default);
            }

            var orderInfo = order.Data.SingleOrDefault();
            if (orderInfo == null)
                return order.AsExchangeError<SharedFuturesTriggerOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            // Return
            return order.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedFuturesTriggerOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, orderInfo.Symbol),
                orderInfo.Symbol,
                orderInfo.OrderId.ToString(),
                orderInfo.TriggerPrice == -1 ? SharedOrderType.Market : SharedOrderType.Limit,
                orderInfo.Side == OrderSide.Buy && orderInfo.PositionSide == PositionSide.Long ? SharedTriggerOrderDirection.Enter : SharedTriggerOrderDirection.Exit,
                orderInfo.Status == TpSlOrderStatus.Failed || orderInfo.Status == TpSlOrderStatus.Canceled ? SharedTriggerOrderStatus.CanceledOrRejected : SharedTriggerOrderStatus.Active,
                orderInfo.TriggerPrice ?? 0,
                orderInfo.PositionSide == PositionSide.Net ? null : orderInfo.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                orderInfo.CreateTime
                )
            {
                OrderQuantity = new SharedOrderQuantity(contractQuantity: orderInfo.Quantity),
                ClientOrderId = orderInfo.ClientOrderId
            });
        }

        EndpointOptions<CancelOrderRequest> IFuturesTriggerOrderRestClient.CancelFuturesTriggerOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> IFuturesTriggerOrderRestClient.CancelFuturesTriggerOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTriggerOrderRestClient)this).CancelFuturesTriggerOrderOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelTriggerOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedId(order.Data.AlgoOrderId.ToString()));
        }

        private OrderSide GetTriggerOrderParameters(SharedTriggerOrderDirection direction, SharedPositionSide side)
        {
            if (direction == SharedTriggerOrderDirection.Enter)
            {
                if (side == SharedPositionSide.Long)
                    // Enter Long = Buy
                    return OrderSide.Buy;
                else
                    // Enter Short = Sell
                    return OrderSide.Sell;
            }

            if (side == SharedPositionSide.Long)
                // Exit Long = Sell
                return OrderSide.Sell;
            else
                // Enter Short = Buy
                return OrderSide.Buy;
        }
        #endregion
    }
}
