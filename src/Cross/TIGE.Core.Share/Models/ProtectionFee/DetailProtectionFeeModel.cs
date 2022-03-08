using System;

namespace TIGE.Core.Share.Models.ProtectionFee
{
    public class CreateProtectionFeeModel
    {
        public double From { get; set; }

        public double To { get; set; }

        public double Fee { get; set; }
    }

    public class DetailProtectionFeeModel: CreateProtectionFeeModel
    {
        public string Id { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
    }
}