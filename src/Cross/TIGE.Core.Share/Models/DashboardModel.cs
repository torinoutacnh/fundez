using System;

namespace TIGE.Core.Share.Models
{
    public class DashboardModel
    {
  
        public int TotalSlots { get; set; }
        public double TotalCommission { get; set; }
        public double TotalToken { get; set; }

    }

    public class StackDashboardModel
    {
        public string WalletAddress { get; set; }
        public double Balance { get; set; }
        public double TotalReward { get; set; }
        public double DailyReward { get; set; }
        public double TotalStack { get; set; }

    }
}