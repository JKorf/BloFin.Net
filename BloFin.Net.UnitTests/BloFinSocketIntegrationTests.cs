using BloFin.Net.Clients;
using BloFin.Net.Objects.Models;
using BloFin.Net.Objects.Options;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BloFin.Net.UnitTests
{
    [NonParallelizable]
    internal class BloFinSocketIntegrationTests : SocketIntegrationTest<BloFinSocketClient>
    {
        public override bool Run { get; set; } = true;

        public BloFinSocketIntegrationTests()
        {
        }

        public override BloFinSocketClient GetClient(ILoggerFactory loggerFactory, bool useUpdatedDeserialization)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");
            var pass = Environment.GetEnvironmentVariable("APIPASS");

            Authenticated = key != null && sec != null && pass != null;
            return new BloFinSocketClient(Options.Create(new BloFinSocketOptions
            {
                OutputOriginalData = true,
                UseUpdatedDeserialization = useUpdatedDeserialization,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec, pass) : null
            }), loggerFactory);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task TestSubscriptions(bool useUpdatedDeserialization)
        {
            //await RunAndCheckUpdate<BloFinSpotTickerUpdate>((client, updateHandler) => client.FuturesApi.SubscribeToWalletUpdatesAsync(default , default), false, true);
            await RunAndCheckUpdate<BloFinTicker>(useUpdatedDeserialization, (client, updateHandler) => client.FuturesApi.SubscribeToTickerUpdatesAsync("ETH-USDT", updateHandler, default), true, false);
            await RunAndCheckUpdate<BloFinOrderBookUpdate>(useUpdatedDeserialization, (client, updateHandler) => client.FuturesApi.SubscribeToOrderBookUpdatesAsync(new string[] { "ETH-USDT" }, 5, updateHandler, default), true, false);
        } 
    }
}
