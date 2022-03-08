using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TIGE.Contract.Repository.Models.Stacking
{
    public class StackCommissionRateEntity : Entity
    {
        public int Level { get; set; }
        public double Rate { get;set;}
        public double Condition { get;set;}
    }
}
