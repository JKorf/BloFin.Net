using BloFin.Net.Clients;
using BloFin.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloFin.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateFuturesAccountCalls()
        {
            var client = new BloFinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<BloFinRestClient>(client, "Endpoints/Futures/Account", "https://openapi.blofin.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetBalancesAsync(AccountType.Futures), "GetBalances", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Account.TransferAsync("USDT", AccountType.Futures, AccountType.Funding, 1), "Transfer", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetAccountConfigAsync(), "GetAccountConfig", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetApiKeyInfoAsync(), "GetApiKeyInfo", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidateFuturesExchangeDataCalls()
        {
            var client = new BloFinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<BloFinRestClient>(client, "Endpoints/Futures/ExchangeData", "https://openapi.blofin.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetSymbolsAsync(), "GetSymbols", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetTickersAsync(), "GetTickers", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetOrderBookAsync("123", 123), "GetOrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetRecentTradesAsync("123", 10), "GetRecentTrades", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetIndexMarkPriceAsync("123"), "GetIndexMarkPrice", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetFundingRateAsync("123"), "GetFundingRate", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetFundingRateHistoryAsync("123"), "GetFundingRateHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.ExchangeData.GetKlinesAsync("123", KlineInterval.OneDay), "GetKlines", nestedJsonProperty: "data");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestHeaders.Any(x => x.Key == "ACCESS-SIGN");
        }
    }
}
