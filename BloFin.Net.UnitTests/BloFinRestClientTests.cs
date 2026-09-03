using BloFin.Net.Clients;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;

namespace BloFin.Net.UnitTests
{
    [TestFixture()]
    public class BloFinRestClientTests
    {
        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<BloFinRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<BloFinSocketClient>();
        }

        [Test]
        public void TestFuturesRestSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = TestHelpers.ValidateSharedApi(new BloFinRestClient().FuturesApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestFuturesSocketSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = TestHelpers.ValidateSharedApi(new BloFinSocketClient().FuturesApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestAccountSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = TestHelpers.ValidateSharedApi(new BloFinRestClient().AccountApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }
    }
}
