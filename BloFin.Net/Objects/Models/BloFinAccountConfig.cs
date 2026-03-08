using BloFin.Net.Enums;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Account config
    /// </summary>
    public record BloFinAccountConfig
    {
        /// <summary>
        /// ["<c>accountLevel</c>"] Account level
        /// </summary>
        [JsonPropertyName("accountLevel")]
        public AccountLevel AccountLevel { get; set; }
    }
}
