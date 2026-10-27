using BloFin.Net.Enums;
using System.Text.Json.Serialization;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Position mode
    /// </summary>
    public record BloFinPositionMode
    {
        /// <summary>
        /// ["<c>positionMode</c>"] Position mode
        /// </summary>
        [JsonPropertyName("positionMode")]
        public PositionMode PositionMode { get; set; }
        /// <summary>
        /// Multi position mode
        /// </summary>
        [JsonPropertyName("multiPosition")]
        public bool MultiPositionMode { get; set; }
    }


}
