using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using CryptoExchange.Net.SharedApis;
using BloFin.Net.Converters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;

namespace BloFin.Net
{
    /// <summary>
    /// BloFin exchange information and configuration
    /// </summary>
    public static class BloFinExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "BloFin";

        /// <summary>
        /// Display name
        /// </summary>
        public static string DisplayName => "BloFin";

        /// <summary>
        /// Url to exchange image
        /// </summary>
        public static string ImageUrl { get; } = "https://raw.githubusercontent.com/JKorf/BloFin.Net/main/BloFin.Net/Icon/icon.png";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://blofin.com/";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://docs.blofin.com"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.CEX;

        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<BloFinSourceGenerationContext>());

        /// <summary>
        /// Aliases for BloFin assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases = [
                new AssetAlias("USDT", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
        };

        /// <summary>
        /// Format a base and quote asset to an BloFin recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());

            return baseAsset + "-" + quoteAsset;
        }

        /// <summary>
        /// Rate limiter configuration for the BloFin API
        /// </summary>
        public static BloFinRateLimiters RateLimiter { get; } = new BloFinRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the BloFin API
    /// </summary>
    public class BloFinRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;
        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal BloFinRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            BloFinRest = new RateLimitGate("BloFin")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, [], 500, TimeSpan.FromMinutes(1), RateLimitWindowType.Sliding, connectionWeight: 0)) // 500 requests per IP per minute
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, [], 1500, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding, connectionWeight: 0)) // 1500 requests per IP per 5 minutes
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerApiKey, new AuthenticatedEndpointFilter(true), 30, TimeSpan.FromSeconds(10), RateLimitWindowType.Sliding)); // 30 requests per user per 10 seconds
            BloFinRest.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            BloFinRest.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);

            BloFinSocket = new RateLimitGate("BloFin")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, [new LimitItemTypeFilter(RateLimitItemType.Connection)], 1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding)); // 1 connection per seconds per ip
            BloFinSocket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            BloFinSocket.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate BloFinRest { get; private set; }
        internal IRateLimitGate BloFinSocket { get; private set; }

    }
}
