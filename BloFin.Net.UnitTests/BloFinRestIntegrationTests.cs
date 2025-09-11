using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using BloFin.Net.Clients;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Objects.Errors;
using System.Threading;

namespace BloFin.Net.UnitTests
{
    [NonParallelizable]
    public class BloFinRestIntegrationTests : RestIntegrationTest<BloFinRestClient>
    {
        public override bool Run { get; set; } = false;

        public override BloFinRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");
            var pass = Environment.GetEnvironmentVariable("APIPASS");

            Authenticated = key != null && sec != null && pass != null;
            return new BloFinRestClient(null, loggerFactory, Options.Create(new BloFinRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec, pass) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().FuturesApi.ExchangeData.GetTickersAsync("TSTTST", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.Code, Is.EqualTo(152002));
            Assert.That(result.Error.ErrorType, Is.EqualTo(ErrorType.InvalidParameter));
        }

        [Test]
        public async Task TestSpotExchangeData()
        {
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetSymbolsAsync(default, CancellationToken.None), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetTickersAsync(default, CancellationToken.None), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetOrderBookAsync("ETH-USDT", default, CancellationToken.None), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetRecentTradesAsync("ETH-USDT", default, CancellationToken.None), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetIndexMarkPriceAsync("ETH-USDT", CancellationToken.None), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetFundingRateAsync("ETH-USDT", CancellationToken.None), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetFundingRateHistoryAsync("ETH-USDT", default, default, default, CancellationToken.None), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetKlinesAsync("ETH-USDT", Enums.KlineInterval.OneDay, default, default, default, CancellationToken.None), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetIndexPriceKlinesAsync("ETH-USDT", Enums.KlineInterval.OneDay, default, default, default, CancellationToken.None), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetMarkPriceKlinesAsync("ETH-USDT", Enums.KlineInterval.OneDay, default, default, default, CancellationToken.None), false);
        }
    }
}
