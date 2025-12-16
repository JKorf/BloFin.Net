using BloFin.Net.Clients;
using BloFin.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BloFin.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateAccountCalls()
        {
            var client = new BloFinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<BloFinRestClient>(client, "Endpoints/Account", "https://openapi.blofin.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.AccountApi.GetAccountBalancesAsync(AccountType.Futures), "GetAccountBalances", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.AccountApi.TransferAsync("USDT", AccountType.Futures, AccountType.Funding, 1), "Transfer", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.AccountApi.GetAccountConfigAsync(), "GetAccountConfig", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.AccountApi.GetApiKeyInfoAsync(), "GetApiKeyInfo", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.AccountApi.GetTransferHistoryAsync(), "GetTransferHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.AccountApi.GetWithdrawalHistoryAsync(), "GetWithdrawalHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.AccountApi.GetDepositHistoryAsync(), "GetDepositHistory", nestedJsonProperty: "data");

        }

        [Test]
        public async Task ValidateFuturesAccountCalls()
        {
            var client = new BloFinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<BloFinRestClient>(client, "Endpoints/Futures/Account", "https://openapi.blofin.com", IsAuthenticated);
            
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetBalancesAsync(), "GetBalances", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetMarginModeAsync(), "GetMarginMode", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Account.SetMarginModeAsync(MarginMode.Isolated), "SetMarginMode", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetPositionModeAsync(), "GetPositionMode", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Account.SetPositionModeAsync(PositionMode.HedgeMode), "SetPositionMode", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Account.GetLeverageAsync("123", MarginMode.Isolated), "GetLeverage", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Account.SetLeverageAsync("123", 0.1m, MarginMode.Isolated), "SetLeverage", nestedJsonProperty: "data");
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

        [Test]
        public async Task ValidateFuturesTradingCalls()
        {
            var client = new BloFinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<BloFinRestClient>(client, "Endpoints/Futures/Trading", "https://openapi.blofin.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetPositionsAsync(), "GetPositions", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.PlaceOrderAsync("123", OrderSide.Buy, OrderType.Limit, 0.1m, MarginMode.Isolated), "PlaceOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.CancelOrderAsync(), "CancelOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetOpenOrdersAsync(), "GetOpenOrders", nestedJsonProperty: "data", ignoreProperties: ["filled_amount"]);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.PlaceMultipleOrdersAsync([]), "PlaceMultipleOrders", nestedJsonProperty: "data", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.PlaceTpSlOrderAsync("123", OrderSide.Buy, MarginMode.Isolated), "PlaceTpSlOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.PlaceTriggerOrderAsync("123", OrderSide.Buy, MarginMode.Isolated,0.1m), "PlaceTriggerOrder", nestedJsonProperty: "data");
            //await tester.ValidateAsync(client => client.FuturesApi.Trading.CancelTpSlOrdersAsync([]), "CancelTpSlOrder", nestedJsonProperty: "data", skipResponseValidation: true);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.CancelTriggerOrderAsync(), "CancelTriggerOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetOpenTpSlOrdersAsync(), "GetOpenTpSlOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetOpenTriggerOrdersAsync(), "GetOpenTriggerOrders", nestedJsonProperty: "data", ignoreProperties: ["orderType"]);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.ClosePositionAsync("123", MarginMode.Isolated), "ClosePosition", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetClosedOrdersAsync(), "GetClosedOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetClosedTpSlOrdersAsync(), "GetClosedTpSlOrders", nestedJsonProperty: "data", ignoreProperties: ["orderType", "triggerType"]);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetClosedTriggerOrdersAsync(), "GetClosedTriggerOrders", nestedJsonProperty: "data", ignoreProperties: ["orderType"]);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetUserTradesAsync(), "GetUserTrades", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetPriceLimitsAsync("123", OrderSide.Buy), "GetPriceLimits", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetOrderAsync("ETHUSDT", "123"), "GetOrder", nestedJsonProperty: "data", ignoreProperties: ["filled_amount"]);
            await tester.ValidateAsync(client => client.FuturesApi.Trading.GetTpSlOrderAsync("ETHUSDT", "123"), "GetTpSlOrder", nestedJsonProperty: "data");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestHeaders.Any(x => x.Key == "ACCESS-SIGN");
        }
    }
}
