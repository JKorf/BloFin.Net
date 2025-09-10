using BloFin.Net.Clients;
using BloFin.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloFin.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateExchangeDataAccountCalls()
        {
            var client = new BloFinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BloFinRestClient>(client, "Endpoints/Spot/ExchangeData", "XXX", IsAuthenticated);
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetTickersAsync(), "GetTickers", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetOrderBookAsync("123", 123), "GetOrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetRecentTradesAsync("123", 10), "GetRecentTrades", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetIndexMarkPriceAsync("123"), "GetIndexMarkPrice", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetFundingRateAsync("123"), "GetFundingRate", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetFundingRateHistoryAsync("123"), "GetFundingRateHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetKlinesAsync("123", KlineInterval.OneDay), "GetKlines", nestedJsonProperty: "data");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestUrl?.Contains("signature") == true || result.RequestBody?.Contains("signature=") == true;
        }
    }
}
