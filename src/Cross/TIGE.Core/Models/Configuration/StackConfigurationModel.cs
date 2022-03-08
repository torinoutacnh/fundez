using System.ComponentModel.DataAnnotations;

namespace TIGE.Core.Models.Configuration
{
    public class StackConfigurationModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }
        [Display(Name = "Wallet Address")]
        public string WalletAddress { get; set; }
        [Display(Name = "About Content")]
        public string AboutPage { get; set; }
        [Display(Name = "Deposit amount")]
        public double DepositAmount { get; set; }
        [Display(Name = "Minimum stacking")]
        public double MinStacking { get; set; }
        [Display(Name = "Minimum withdraw")]
        public double MinWithDraw { get; set; }
        [Display(Name = "Minimum Convert")]
        public double MinWithDrawUSD { get; set; }
        [Display(Name = "Minimum Transfer")]
        public double MinTransfer { get; set; }
        [Display(Name = "Contact Address")]
        public string ContactAddress { get; set; }
        [Display(Name = "Support Email")]
        public string SupportEmail { get; set; }
        [Display(Name = "Telegram")]
        public string TelegramLink { get; set; }
        [Display(Name = "TIGE Chart Range (Month)")]
        public int TIGEChartRange { get; set; }
    }
}
