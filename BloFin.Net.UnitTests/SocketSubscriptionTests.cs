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
        public async Task ValidateSpotExchangeDataSubscriptions()
        {
            var client = new BloFinSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<BloFinSocketClient>(client, "Subscriptions/Spot", "XXX");
            //await tester.ValidateAsync<BloFinModel>((client, handler) => client.SpotApi.SubscribeToXXXUpdatesAsync(handler), "XXX");
        }
    }
}
