using System;
using Elect.Web.DataTable.Attributes;
using Elect.Web.DataTable.Models.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.Crypto;

namespace TIGE.Core.Share.Models.Slot
{
    public class SubmitSlotModel
    {
        public int Quantity { get; set; }
    }

    public class DetailSlotRequestModel
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public double TokenQuantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalAmount { get; set; }
        public string ApproveName { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public double CurrentBalance { get; set; }
        [DataTable(IsSortable = true, SortDirection = SortDirection.Descending)]
        public DateTimeOffset Time { get; set; }
        public Enums.SlotStatus Status { get; set; }
    }
}