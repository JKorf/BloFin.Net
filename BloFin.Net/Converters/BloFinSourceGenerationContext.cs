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
    [JsonSerializable(typeof(BloFinResponse))]

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
