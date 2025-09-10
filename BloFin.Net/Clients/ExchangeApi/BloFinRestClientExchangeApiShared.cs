using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;
using BloFin.Net.Interfaces.Clients.ExchangeApi;

namespace BloFin.Net.Clients.ExchangeApi
{
    internal partial class BloFinRestClientExchangeApi : IBloFinRestClientExchangeApiShared
    {
        private const string _topicId = "BloFinExchange";
        public string Exchange => "BloFin";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
    }
}
