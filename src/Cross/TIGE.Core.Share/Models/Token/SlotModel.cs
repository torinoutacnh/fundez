using System;
using Elect.Web.DataTable.Attributes;
using Elect.Web.DataTable.Models.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.Crypto;

namespace TIGE.Core.Share.Models.Token
{
    public class SubmitTokenModel
    {
        public int Quantity { get; set; }
    }

    public class SellTokenPriceModel
    {
        public double Quantity { get; set; }
        public double FeeAmount { get; set; }
        public double TotalAmount { get; set; }

        public double UnitPrice { get; set; }
    }

    public class DetailSellTokenModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public double TokenQuantity { get; set; }

        public double UnitPrice { get; set; }
        public double FeeAmount { get; set; }
        public double TotalAmount { get; set; }

        public DateTimeOffset? ApproveTime { get; set; }
        public string ApproveBy { get; set; }
        public string ApproveName { get; set; }

        [DataTable(IsSortable = true, SortDirection = SortDirection.Descending)]
        public DateTimeOffset CreatedTime { get; set; }
        public Enums.TokenStatus Status { get; set; }
    }
}