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
        #region Futures Symbol client
        public SharedSymbolCatalog? FuturesSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicId, _api.EnvironmentName, null);

        public GetFuturesSymbolsOptions GetFuturesSymbolsOptions { get; } = new GetFuturesSymbolsOptions(_exchangeName, false);
        public async Task<HttpResult<SharedFuturesSymbol[]>> GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesSymbol[]>(Exchange, validationError);

            var symbolsResult = await _api.ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!symbolsResult.Success)
                return HttpResult.Fail<SharedFuturesSymbol[]>(symbolsResult);

            var data = symbolsResult.Data
                .Select(x => ParseSymbol(x))
                .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, _api.EnvironmentName, null, data);
            return HttpResult.Ok(symbolsResult, SharedUtils.ApplySymbolFilter(data, request));
        }

        private SharedFuturesSymbol ParseSymbol(BloFinSymbol s)
        {
            var result = new SharedFuturesSymbol(s.ContractType == ContractType.Linear ? TradingMode.PerpetualLinear : TradingMode.PerpetualInverse,
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
                MaxShortLeverage = s.MaxLeverage,
                DisplayName = s.Symbol
            };

            result.QuoteAssetType = result.TradingMode == TradingMode.PerpetualInverse ? SharedAssetType.Fiat : SharedAssetType.Crypto;
            result.QuoteAssetSubType = result.TradingMode == TradingMode.PerpetualInverse ? null : SharedAssetSubType.StableCoin;
            if (s.AssetClass == AssetClass.Crypto)
            {
                result.BaseAssetType = SharedAssetType.Crypto;
                if (LibraryHelpers.IsStableCoin(result.BaseAsset))
                    result.BaseAssetSubType = SharedAssetSubType.StableCoin;
            }
            else if (s.AssetClass == AssetClass.Commodities)
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Commodity;
            }
            else if (s.AssetClass == AssetClass.Stocks)
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Equity;
            }
            else if (s.AssetClass == AssetClass.Indices)
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Equity;
            }
            else
            {
                result.BaseAssetType = SharedAssetType.Unspecified;
            }

            return result;
        }

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetFuturesSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsFuturesSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Spot symbols not allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsFuturesSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetFuturesSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbolName));
        }
        #endregion
    }
}
