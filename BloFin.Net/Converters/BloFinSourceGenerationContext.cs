using BloFin.Net.Objects.Internal;
using BloFin.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BloFin.Net.Converters
{
    [JsonSerializable(typeof(BloFinResponse<BloFinIndexMarkKline[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinKline[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinFundingRate[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinMarkIndexPrice[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinTrade[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinSymbol[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinTicker[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinOrderBook[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinBalance[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinTransferResult>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinAccountConfig>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinApiKey>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinTransfer[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinWithdrawal[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinDeposit[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinFuturesBalances>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinPosition[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinMarginMode>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinPositionMode>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinLeverage[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinLeverage>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinOrderId[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinOrderId>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinOrder[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinTpSlOrderId>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinAlgoOrderId>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinCancelResponse[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinTpSlOrderId[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinTpSlOrder[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinAlgoOrder[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinClosePositionResult[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinUserTrade[]>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinPriceLimit>))]
    [JsonSerializable(typeof(BloFinResponse<BloFinClosePositionResult>))]
    [JsonSerializable(typeof(BloFinResponse))]

    [JsonSerializable(typeof(BloFinSocketRequest))]
    [JsonSerializable(typeof(BloFinSocketResponse))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinTrade[]>))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinKline[]>))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinIndexMarkKline[]>))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinOrderBookUpdate>))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinTicker[]>))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinFundingRate[]>))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinPosition[]>))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinOrder[]>))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinAlgoOrderUpdate[]>))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinFuturesBalances>))]
    [JsonSerializable(typeof(BloFinSocketUpdate<BloFinFuturesInverseBalanceUpdate>))]

    [JsonSerializable(typeof(BloFinOrderRequest[]))]
    [JsonSerializable(typeof(BloFinCancelRequest[]))]
    [JsonSerializable(typeof(BloFinTpSlOrderRequest[]))]
    [JsonSerializable(typeof(BloFinCancelTpSlRequest[]))]

    [JsonSerializable(typeof(Dictionary<string, object>))]
    [JsonSerializable(typeof(IDictionary<string, object>))]

    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    internal partial class BloFinSourceGenerationContext : JsonSerializerContext
    {
    }
}
