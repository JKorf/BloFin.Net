using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloFin.Net.Objects.Models
{
    /// <summary>
    /// Asset info
    /// </summary>
    public record BloFinAsset
    {
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>chain</c>"] Network
        /// </summary>
        [JsonPropertyName("chain")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>depositMinAmount</c>"] Min deposit
        /// </summary>
        [JsonPropertyName("depositMinAmount")]
        public decimal MinDeposit { get; set; }
        /// <summary>
        /// ["<c>depositUnsafeConfirmation</c>"] Confirmation count
        /// </summary>
        [JsonPropertyName("depositUnsafeConfirmation")]
        public int DepositUnsafeConfirmations { get; set; }
        /// <summary>
        /// ["<c>depositConfirmation</c>"] Confirmation count
        /// </summary>
        [JsonPropertyName("depositConfirmation")]
        public int DepositConfirmation { get; set; }
        /// <summary>
        /// ["<c>withdrawMinAmount</c>"] Min withdraw quantity
        /// </summary>
        [JsonPropertyName("withdrawMinAmount")]
        public decimal MinWithdraw { get; set; }
        /// <summary>
        /// ["<c>withdrawFee</c>"] Withdraw fee
        /// </summary>
        [JsonPropertyName("withdrawFee")]
        public decimal WithdrawFee { get; set; }
        /// <summary>
        /// ["<c>withdrawPrecision</c>"] Withdraw precision
        /// </summary>
        [JsonPropertyName("withdrawPrecision")]
        public int WithdrawPrecision { get; set; }
        /// <summary>
        /// ["<c>supportMemo</c>"] Memo/tag supported
        /// </summary>
        [JsonPropertyName("supportMemo")]
        public bool SupportsMemo { get; set; }
        /// <summary>
        /// ["<c>isDepositAvailable</c>"] Deposit available
        /// </summary>
        [JsonPropertyName("isDepositAvailable")]
        public bool DepositAvailable { get; set; }
        /// <summary>
        /// ["<c>isWithdrawAvailable</c>"] Withdraw available
        /// </summary>
        [JsonPropertyName("isWithdrawAvailable")]
        public bool WithdrawAvailable { get; set; }
        /// <summary>
        /// ["<c>recoveryEtaDeposit</c>"] Recover time estimate
        /// </summary>
        [JsonPropertyName("recoveryEtaDeposit")]
        public int RecoveryEtaDeposit { get; set; }
        /// <summary>
        /// ["<c>recoveryEtaWithdraw</c>"] Recover time estimate
        /// </summary>
        [JsonPropertyName("recoveryEtaWithdraw")]
        public int RecoveryEtaWithdrawal { get; set; }
        /// <summary>
        /// ["<c>logo</c>"] Logo
        /// </summary>
        [JsonPropertyName("logo")]
        public string Logo { get; set; } = string.Empty;
    }
}
