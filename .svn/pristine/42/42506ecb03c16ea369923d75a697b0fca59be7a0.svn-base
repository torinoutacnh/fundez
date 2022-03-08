using System;
using Elect.Web.DataTable.Attributes;
using Elect.Web.DataTable.Models.Constants;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Share.Models
{
    public class BusinessDetailModel
    {

        public string Id { get; set; }

        public string From { get; set; }
        public string To { get; set; }

        [DataTable(IsSortable = true, SortDirection = SortDirection.Descending)]
        public DateTimeOffset Date { get; set; }

        public double AmountUSD { get; set; }
        public int AmountSlot { get; set; }
        public double Commission { get; set; }

        public Enums.BusinessStatus Status { get; set; }
    }    
    
    

}