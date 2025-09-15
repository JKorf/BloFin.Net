using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Cancel request
    /// </summary>
    public record BloFinCancelTpSlRequest
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("instId"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Symbol { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientOrderId"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// TPSL Order id
        /// </summary>
        [JsonPropertyName("tpslId"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? OrderId { get; set; }
    }
}
