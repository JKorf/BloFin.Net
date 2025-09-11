using BloFin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Account config
    /// </summary>
    public record BloFinAccountConfig
    {
        /// <summary>
        /// Account level
        /// </summary>
        [JsonPropertyName("accountLevel")]
        public AccountLevel AccountLevel { get; set; }
    }
}
