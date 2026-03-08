using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Price limits
    /// </summary>
    public record BloFinPriceLimit
    {
        /// <summary>
        /// ["<c>maxPrice</c>"] Max price
        /// </summary>
        [JsonPropertyName("maxPrice")]
        public decimal MaxPrice { get; set; }
        /// <summary>
        /// ["<c>minPrice</c>"] Min price
        /// </summary>
        [JsonPropertyName("minPrice")]
        public decimal MinPrice { get; set; }
    }


}
