using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Threading.Tasks;
using BloFin.Net.Clients;
using BloFin.Net.Objects.Models;

namespace BloFin.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateFuturesExchangeDataSubscriptions()
        {
            var client = new BloFinSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456", "789");
            });
            var tester = new SocketSubscriptionValidator<BloFinSocketClient>(client, "Subscriptions/Futures", "wss://openapi.blofin.com");
            await tester.ValidateAsync<BloFinTrade[]>((client, handler) => client.FuturesApi.SubscribeToTradeUpdatesAsync("ETH-USDT", handler), "Trades", nestedJsonProperty: "data");
            await tester.ValidateAsync<BloFinKline>((client, handler) => client.FuturesApi.SubscribeToKlineUpdatesAsync("ETH-USDT", Enums.KlineInterval.OneDay, handler), "Klines", useFirstUpdateItem: true, nestedJsonProperty: "data");
            await tester.ValidateAsync<BloFinIndexMarkKline>((client, handler) => client.FuturesApi.SubscribeToIndexPriceKlineUpdatesAsync("ETH-USDT", Enums.KlineInterval.OneDay, handler), "IndexKlines", useFirstUpdateItem: true, nestedJsonProperty: "data");
            await tester.ValidateAsync<BloFinIndexMarkKline>((client, handler) => client.FuturesApi.SubscribeToMarkPriceKlineUpdatesAsync("ETH-USDT", Enums.KlineInterval.OneDay, handler), "MarkKlines", useFirstUpdateItem: true, nestedJsonProperty: "data");
            await tester.ValidateAsync<BloFinOrderBookUpdate>((client, handler) => client.FuturesApi.SubscribeToOrderBookUpdatesAsync("ETH-USDT", 5, handler), "OrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync<BloFinTicker>((client, handler) => client.FuturesApi.SubscribeToTickerUpdatesAsync("ETH-USDT", handler), "Ticker", useFirstUpdateItem: true, nestedJsonProperty: "data");
            await tester.ValidateAsync<BloFinFundingRate>((client, handler) => client.FuturesApi.SubscribeToFundingRateUpdatesAsync("ETH-USDT", handler), "FundingRate", useFirstUpdateItem: true, nestedJsonProperty: "data");
            
            await tester.ValidateAsync<BloFinPosition[]>((client, handler) => client.FuturesApi.SubscribeToPositionUpdatesAsync(handler), "Positions", nestedJsonProperty: "data");
            await tester.ValidateAsync<BloFinOrder[]>((client, handler) => client.FuturesApi.SubscribeToOrderUpdatesAsync(handler), "Orders", nestedJsonProperty: "data");
            await tester.ValidateAsync<BloFinAlgoOrderUpdate[]>((client, handler) => client.FuturesApi.SubscribeToTriggerOrderUpdatesAsync(handler), "TriggerOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync<BloFinFuturesBalances>((client, handler) => client.FuturesApi.SubscribeToBalanceUpdatesAsync(handler), "Balances", nestedJsonProperty: "data");
            await tester.ValidateAsync<BloFinFuturesInverseBalanceUpdate>((client, handler) => client.FuturesApi.SubscribeToInverseBalanceUpdatesAsync(handler), "InverseBalances", nestedJsonProperty: "data");
        }
    }
}
