using System.Collections.Generic;
using TIGE.Contract.Repository.Models;

namespace TIGE.Contract.Repository.Models.Stacking
{
    public class SubscriptionEntity : Entity
    {
        public int Day { get; set; }
        public double Reward { get; set; }
        public string Description { get; set; }

        public ICollection<StackHistoryEntity> RefundHistories { get; set; }
    }
}
